using System.Data.SQLite;

ReadData(CreateConnection());
InsertCustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
//FindCustomer(CreateConnection());
//DisplayProduct(CreateConnection());


static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source = mydb.db; Version = 3; New = True; Compress = True;");

    try
    {
        connection.Open();
        Console.WriteLine("DB found.");
    }
    catch
    {
        Console.WriteLine("DB not found.");
    }

    return connection;
}

static void ReadData(SQLiteConnection myconnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myconnection.CreateCommand();
    command.CommandText = "select rowid, * from customer";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowID = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowID}. Full name: {readerStringFirstName} {readerStringLastName}; Status: {readerStringDoB}");
    }

    myconnection.Close();
}

static void InsertCustomer(SQLiteConnection myconnection)
{
    SQLiteCommand command;
    string fName, lName, dob;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-yyyy):");
    dob = Console.ReadLine();

    command = myconnection.CreateCommand();
    command.CommandText = $"\r\nINSERT INTO customer (firstName,lastName,dateOfBirth) VALUES (\r\n  '{fName}',\r\n  '{lName}',\r\n  '{dob}'\r\n)";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");

    ReadData(myconnection);

}

static void RemoveCustomer(SQLiteConnection myconnection)
{
    SQLiteCommand command;
    string idToDelete;

    Console.WriteLine("Enter an ID to delete a customer:");
    idToDelete = Console.ReadLine();

    command = myconnection.CreateCommand();
    command.CommandText = $"delete from customer \r\nwhere rowid = {idToDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table 'customer'.");

    ReadData(myconnection);
}

static void FindCustomer(SQLiteConnection myconnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");
    searchName = Console.ReadLine();

    command = myconnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstName, customer.lastName, status.statusType " +
        $"FROM customerStatus " +
        $"JOIN customer ON customer.rowid = customerStatus.customerId " +
        $"JOIN status ON status.rowid = customerStatus.statusId " +
        $"WHERE customer.FirstName LIKE '{searchName}%'";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowID = reader["rowid"].ToString();
        string readerStringName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);

        Console.WriteLine($"Search result: ID: {readerRowID}. {readerStringName} {readerStringLastName}. Status: {readerStringStatus}");
    }

    myconnection.Close();
}

static void DisplayProduct(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, ProductName, Price FROM product";
    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerProductName = reader.GetString(1);
        int readerProductPrice = reader.GetInt32(2); // Hinna tüüp andmebaasis on int, nii et siin loeme andmebaasis ka int-tüüpi andmeid

        Console.WriteLine($"{readerRowid}. {readerProductName}. Price: {readerProductPrice}");
    }

    reader.Close();
}