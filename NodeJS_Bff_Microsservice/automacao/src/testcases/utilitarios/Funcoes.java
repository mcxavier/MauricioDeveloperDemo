package utilitarios;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Properties;

public class Funcoes {

		public static String ReturnEnvironment() {
			String vEnvironment;
			vEnvironment = System.getProperty("environment");
			return vEnvironment;
		}
		
		public static Properties getprop() throws IOException {
			
			Properties props = new Properties();	
			FileInputStream file = new FileInputStream("./properties/dados.properties");
			props.load(file);
			
			return props;
		}
}
