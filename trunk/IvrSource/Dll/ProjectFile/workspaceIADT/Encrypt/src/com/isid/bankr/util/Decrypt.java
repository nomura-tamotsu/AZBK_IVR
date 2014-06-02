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
			// �閧�L�[�����񂩂�閧�L�[�𐶐�
			SecretKeyFactory keyFac = SecretKeyFactory.getInstance("DESede");
			DESedeKeySpec keySpec = new DESedeKeySpec(Constants.key.getBytes("UTF-8"));
			SecretKey secKey = keyFac.generateSecret(keySpec);

			// �������x�N�^(Initial Vector)����
			IvParameterSpec IV = new IvParameterSpec(Constants.ivStr.getBytes("UTF-8"));

			// �Í����I�u�W�F�N�g����(DESede CBC PKCS5Padding)
			Cipher ch = Cipher.getInstance("DESede/CBC/PKCS5Padding");
			ch.init(Cipher.DECRYPT_MODE, secKey, IV);
			
			//Base64�G���R�[�h���ꂽ��������f�R�[�h
			byte[] encrypted = Base64.decode(base64str);
			
			// ������
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
