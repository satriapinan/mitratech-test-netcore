using System;
using System.Collections.Generic;
using System.Linq;

// Aplikasi ini adalah program CRUD sederhana untuk mengelola data karyawan.
// User dapat menambah, melihat, memperbarui, dan menghapus data karyawan.
// Program juga sudah memiliki validasi untuk input yang valid dan error handling.

public class Program : IEmployeeOperations
{
  // List untuk menyimpan data karyawan
  static List<Employee> employees = new List<Employee>();

  static void Main(string[] args)
  {
    Program program = new Program(); // Membuat satu instance untuk digunakan di seluruh program
    SeedData();  // Inisialisasi data karyawan
    bool running = true;

    // Loop untuk menampilkan menu dan menjalankan program sampai user memilih Exit
    while (running)
    {
      int choice = ShowMenu();  // Menampilkan menu pilihan

      // Menjalankan opsi sesuai pilihan user
      switch (choice)
      {
        case 0:
          program.CreateEmployee();  // Tambah karyawan baru
          break;
        case 1:
          program.ReadEmployees();  // Tampilkan daftar karyawan
          break;
        case 2:
          program.UpdateEmployee();  // Update data karyawan
          break;
        case 3:
          program.DeleteEmployee();  // Hapus karyawan
          break;
        case 4:
          running = false;  // Keluar dari program
          Console.WriteLine("\nExiting...");
          break;
        default:
          Console.WriteLine("\nInvalid option. Try again.");  // Jika pilihan tidak valid (karena sudah menggunakan arrow seharusnya tidak ada)
          break;
      }
    }
  }

  // Fungsi untuk menampilkan menu dan menangani input dari keyboard (panah atas/bawah)
  static int ShowMenu()
  {
    string[] options = { "Create Employee", "Read Employees", "Update Employee", "Delete Employee", "Exit" };
    int selectedIndex = 0;

    ConsoleKeyInfo keyInfo;
    do
    {
      Console.Clear();
      Console.WriteLine("Choose an option:");

      // Tampilkan pilihan menu
      for (int i = 0; i < options.Length; i++)
      {
        if (i == selectedIndex)
        {
          Console.ForegroundColor = ConsoleColor.Green;  // Warna hijau untuk pilihan yang disorot
          Console.WriteLine($"> {options[i]}");
          Console.ResetColor();
        }
        else
        {
          Console.WriteLine($"  {options[i]}");
        }
      }

      keyInfo = Console.ReadKey();  // Baca input keyboard

      // Navigasi menggunakan panah atas dan bawah
      if (keyInfo.Key == ConsoleKey.UpArrow)
      {
        selectedIndex--;
        if (selectedIndex < 0) selectedIndex = options.Length - 1;
      }
      else if (keyInfo.Key == ConsoleKey.DownArrow)
      {
        selectedIndex++;
        if (selectedIndex >= options.Length) selectedIndex = 0;
      }

    } while (keyInfo.Key != ConsoleKey.Enter);  // Tunggu sampai user menekan Enter

    return selectedIndex;  // Kembalikan pilihan yang dipilih
  }

  // Inisialisasi data karyawan awal
  static void SeedData()
  {
    employees.Add(new Employee(1001, "Adit", new DateTime(1954, 8, 17)));
    employees.Add(new Employee(1002, "Anton", new DateTime(1954, 8, 18)));
    employees.Add(new Employee(1003, "Amir", new DateTime(1954, 8, 19)));
  }

  // Fungsi untuk menambahkan karyawan baru
  public void CreateEmployee()
  {
    try
    {
      Console.Write("\nEnter Employee ID: ");
      int id = Convert.ToInt32(Console.ReadLine());

      if (employees.Any(e => e.EmployeeID == id))
      {
        Console.WriteLine("Employee ID already exists.");
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        return;
      }

      Console.Write("Enter Full Name: ");
      string name = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(name))
      {
        Console.WriteLine("Name cannot be empty.");
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        return;
      }

      Console.Write("Enter Birth Date (dd-MM-yyyy): ");
      DateTime birthDate;
      if (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out birthDate))
      {
        Console.WriteLine("Invalid date format.");
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        return;
      }

      employees.Add(new Employee(id, name, birthDate));  // Tambahkan karyawan baru ke dalam list
      Console.WriteLine("\nEmployee added successfully.\n");
    }
    catch (FormatException)
    {
      Console.WriteLine("Invalid input format. Please enter correct data.");
      Console.WriteLine("\nPress any key to return to menu...");
      Console.ReadKey();
      return;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred: {ex.Message}");
      Console.WriteLine("\nPress any key to return to menu...");
      Console.ReadKey();
      return;
    }

    ReadEmployees();  // Tampilkan daftar karyawan setelah penambahan
  }

  // Fungsi untuk menampilkan daftar karyawan dalam bentuk tabel
  public void ReadEmployees()
  {
    Console.WriteLine("\nEmployee List:");
    if (employees.Count == 0)
    {
      Console.WriteLine("\nNo employees found.");  // Jika tidak ada karyawan
    }
    else
    {
      // Hitung lebar kolom berdasarkan konten terpanjang
      int idWidth = Math.Max("EmployeeID".Length, MaxLength(e => e.EmployeeID.ToString()));
      int nameWidth = Math.Max("FullName".Length, MaxLength(e => e.FullName));
      int birthDateWidth = Math.Max("BirthDate".Length, MaxLength(e => e.BirthDate.ToString("dd-MMM-yyyy")));

      // Format tabel
      string headerFormat = $"| {{0,-{idWidth}}} | {{1,-{nameWidth}}} | {{2,-{birthDateWidth}}} |";
      string line = new string('-', idWidth + nameWidth + birthDateWidth + 10);  // Garis pemisah tabel
      Console.WriteLine(line);
      Console.WriteLine(headerFormat, "EmployeeID", "FullName", "BirthDate");
      Console.WriteLine(line);

      // Tampilkan data karyawan
      foreach (var emp in employees)
      {
        Console.WriteLine(headerFormat, emp.EmployeeID, emp.FullName, emp.BirthDate.ToString("dd-MMM-yyyy"));
      }
      Console.WriteLine(line);
    }
    Console.WriteLine("\nPress any key to return to menu...");
    Console.ReadKey();  // Tunggu sampai user menekan tombol sebelum kembali ke menu
  }

  // Fungsi untuk menghitung panjang maksimal dari konten kolom
  static int MaxLength(Func<Employee, string> selector)
  {
    int maxLength = 0;
    foreach (var emp in employees)
    {
      int length = selector(emp).Length;
      if (length > maxLength)
        maxLength = length;
    }
    return maxLength;  // Kembalikan panjang maksimal
  }

  // Fungsi untuk mengupdate data karyawan
  public void UpdateEmployee()
  {
    try
    {
      Console.Write("\nEnter Employee ID to update: ");
      int id = Convert.ToInt32(Console.ReadLine());

      var emp = employees.Find(e => e.EmployeeID == id);  // Cari karyawan berdasarkan ID
      if (emp != null)
      {
        Console.Write("Enter new Full Name: ");
        string newName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newName))
        {
          Console.WriteLine("Name cannot be empty.");
          Console.WriteLine("\nPress any key to return to menu...");
          Console.ReadKey();
          return;
        }
        emp.FullName = newName;

        Console.Write("Enter new Birth Date (dd-MM-yyyy): ");
        DateTime newBirthDate;
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out newBirthDate))
        {
          Console.WriteLine("Invalid date format.");
          Console.WriteLine("\nPress any key to return to menu...");
          Console.ReadKey();
          return;
        }
        emp.BirthDate = newBirthDate;

        Console.WriteLine("Employee updated successfully.\n");
      }
      else
      {
        Console.WriteLine("Employee not found.");
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        return;
      }
    }
    catch (FormatException)
    {
      Console.WriteLine("Invalid input format. Please enter correct data.");
      Console.WriteLine("\nPress any key to return to menu...");
      Console.ReadKey();
      return;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred: {ex.Message}");
      Console.WriteLine("\nPress any key to return to menu...");
      Console.ReadKey();
      return;
    }

    ReadEmployees();  // Tampilkan daftar karyawan setelah update
  }

  // Fungsi untuk menghapus karyawan
  public void DeleteEmployee()
  {
    try
    {
      Console.Write("\nEnter Employee ID to delete: ");
      int id = Convert.ToInt32(Console.ReadLine());

      var emp = employees.Find(e => e.EmployeeID == id);  // Cari karyawan berdasarkan ID
      if (emp != null)
      {
        employees.Remove(emp);  // Hapus karyawan dari list
        Console.WriteLine("Employee deleted successfully.\n");
      }
      else
      {
        Console.WriteLine("Employee not found.");
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        return;
      }
    }
    catch (FormatException)
    {
      Console.WriteLine("Invalid input format. Please enter correct data.");
      Console.WriteLine("\nPress any key to return to menu...");
      Console.ReadKey();
      return;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred: {ex.Message}");
      Console.WriteLine("\nPress any key to return to menu...");
      Console.ReadKey();
      return;
    }

    ReadEmployees();  // Tampilkan daftar karyawan setelah penghapusan
  }
}
