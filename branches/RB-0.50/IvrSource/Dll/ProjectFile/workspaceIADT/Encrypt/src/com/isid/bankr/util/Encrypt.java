package com.isid.bankr.util;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.DESedeKeySpec;
import javax.crypto.spec.IvParameterSpec;

//import com.sun.org.apache.xml.internal.security.utils.Base64;
import sun.misc.BASE64Encoder;

public class Encrypt  {
	private String password  = "";
	private String errorMsg = "";
	
	public int encrypt(String originalCode) {

		StringBuffer sb = new StringBuffer();
		
		try {
			
			// 暗号化文字列をUTF-8のバイトコードにに変換
			byte[] m = originalCode.getBytes("UTF-8");

			for (int i = 0; i < m.length; ++i) {
				sb.append(Integer.toHexString(m[i]) + " ");
			}

			// 秘密キー文字列から秘密キーを生成
			SecretKeyFactory keyFac = SecretKeyFactory.getInstance("DESede");
			DESedeKeySpec keySpec = new DESedeKeySpec(Constants.key.getBytes("UTF-8"));
			SecretKey secKey = keyFac.generateSecret(keySpec);

			// 初期化ベクタ(Initial Vector)生成
			IvParameterSpec IV = new IvParameterSpec(Constants.ivStr.getBytes("UTF-8"));

			// 暗号化オブジェクト生成(DESede CBC PKCS5Padding)
			Cipher ch = Cipher.getInstance("DESede/CBC/PKCS5Padding");
			ch.init(Cipher.ENCRYPT_MODE, secKey, IV);

			// 暗号化
			byte[] c = ch.doFinal(m);

			// 暗号化バイトを符号無8bit化
			StringBuffer sb2 = new StringBuffer();
			for (int i = 0; i < c.length; ++i) {
				sb2.append(Integer.toHexString(c[i] & 0xff) + " ");
			}
			
			// base64エンコード
//			String base64str = encode(c);
//			String base64str = Base64.encode(c);
			BASE64Encoder enc = new BASE64Encoder();
			String base64str = enc.encode(c);
			
			this.setPassword(base64str);
			return 0;
			
		} catch (Exception e) {
			//System.out.println(e);
			String errMsg = "Message=" + e.getMessage() + "; Exception=" + e.toString();
			this.setErrorMsg(errMsg);
			return 9;
		}
		
	}
	
//	private static char[] base64EncodeTable = new char[]{
//		'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
//		'Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f',
//		'g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v',
//		'w','x','y','z','0','1','2','3','4','5','6','7','8','9','+','/'
//	};
//	
//	
//		private static String encode(byte[] buf){
//		StringBuffer encoded = new StringBuffer(buf.length * 4 / 3 );
//		
//		char[] c =  new char[4];
//		
//		int i = 0;
//		while(i < buf.length - 2) {
//			c[0] = base64EncodeTable[(buf[i] & 0xfc) >> 2];
//			c[1] = base64EncodeTable[((buf[i] & 0x03) << 4) | ((buf[++i] & 0xf0) >> 4)];
//			c[2] = base64EncodeTable[((buf[i] & 0x0f) << 2) | ((buf[++i] & 0xc0) >> 6)];
//			c[3] = base64EncodeTable[buf[i] & 0x3f];
//			
//
//			encoded.append(c);
//			i++;
//			
///*	今回は改行なしであつかう。
//			if (i % 57 == 0) {
//				encoded.append("\n");	// 改行コード（LF）を入れる。
//			}
//*/		}
//		
//		if (i == buf.length - 2) {
//			c[0] = base64EncodeTable[(buf[i] & 0xfc) >> 2];
//			c[1] = base64EncodeTable[((buf[i] & 0x03) << 4) | ((buf[++i] & 0xf0) >> 4)];
//			c[2] = base64EncodeTable[(buf[i] & 0x0f) << 2];
//			c[3] = '=';
//			encoded.append(c);
//			
//		} else if (i == buf.length - 1 ) {
//			c[0] = base64EncodeTable[(buf[i] & 0xfc) >> 2];
//			c[1] = base64EncodeTable[(buf[i] & 0x03) << 4];
//			c[2] = '=';
//			c[3] = '=';
//			encoded.append(c);
//			
//		}
//
//		return encoded.toString();
//	}
	

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
		Encrypt enc = new Encrypt();
		String data = "1234";
		
		int ret = enc.encrypt(data);
		String password = enc.getPassword();
		String errMsg = enc.getErrorMsg();
		
		return;
		
//		for(int i=0; i<10000; i++){
//			String orgCode = String.format("%1$04d", i);
//			Encrypt enc = new Encrypt();
//			enc.encrypt(orgCode);
//			String password = enc.getPassword();
//			System.out.println("暗号化：" + orgCode + "→" + password);
//			Decrypt dec = new Decrypt();
//			dec.decrypt(password);
//			String password2 = dec.getPassword();
//
//			System.out.println("復号化：" + password + "→" + password2);
//			
//			System.out.println("============================");			
//		}
	
	}
}

