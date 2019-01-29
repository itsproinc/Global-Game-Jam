package daspro1;

import java.util.Scanner;

public class hitungWaktu {
	
	public static void main(String[] args)
	{
		Scanner sc = new Scanner(System.in);
		float S, V, T;
		
		System.out.print("Ketikkan jarak: ");  S = sc.nextFloat();
		System.out.print("Ketikkan kecepatan: ");  V = sc.nextFloat();
		T = S/V;
		System.out.print("Waktu: " + T);
	}
}
