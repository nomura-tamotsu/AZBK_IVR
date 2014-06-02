package com.isid.bankr.util;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.DESedeKeySpec;
import javax.crypto.spec.IvParameterSpec;

import com.sun.org.apache.xml.internal.security.utils.Base64;

public class Decrypt {
	private String password  = "";
	private String errorMsg = "";

	public int decrypt(String base64str) {
		try {
			// 秘密キー文字列から秘密キーを生成
			SecretKeyFactory keyFac = SecretKeyFactory.getInstance("DESede");
			DESedeKeySpec keySpec = new DESedeKeySpec(Constants.key.getBytes("UTF-8"));
			SecretKey secKey = keyFac.generateSecret(keySpec);

			// 初期化ベクタ(Initial Vector)生成
			IvParameterSpec IV = new IvParameterSpec(Constants.ivStr.getBytes("UTF-8"));

			// 暗号化オブジェクト生成(DESede CBC PKCS5Padding)
			Cipher ch = Cipher.getInstance("DESede/CBC/PKCS5Padding");
			ch.init(Cipher.DECRYPT_MODE, secKey, IV);
			
			//Base64エンコードされた文字列をデコード
			byte[] encrypted = Base64.decode(base64str);
			
			// 復号化
			byte[] decrypted = ch.doFinal(encrypted);
			
			String originalCode = new String(decrypted, "UTF-8");
			
			this.setPassword(originalCode);
			
			return 0;
			
		} catch (Exception e) {
			//System.out.println(e);
			String errMsg = "Message=" + e.getMessage() + "; Exception=" + e.toString();
			this.setErrorMsg(errMsg);
			return 9;
		}
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getErrorMsg() {
		return errorMsg;
	}

	public void setErrorMsg(String errorMsg) {
		this.errorMsg = errorMsg;
	}

	public static void main(String[] args) {
		Decrypt dec = new Decrypt();
		String data = "fnkf8HPDZH8=";
		
		int ret = dec.decrypt(data);
		String password = dec.getPassword();
		String errMsg = dec.getErrorMsg();
		
		return;
	}

}
