package daspro1;

import java.util.Scanner;

public class daspro1 {
	
	public static void main(String[] args)
	{
		Scanner sc = new Scanner(System.in);
		String nama = "Bakhtiar";
		
		System.out.print("Ketikkan nama: "); 
		nama = sc.next();
		System.out.println("hello " + nama);
	}
}