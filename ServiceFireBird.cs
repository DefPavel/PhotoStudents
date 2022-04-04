internal class ServiceFireBird
{
    const string Connection = "database=192.168.250.72:Cont;user=sysdba;password=Vtlysq~Bcgjkby2020;Charset=win1251;";
    public static byte[] GetBytes(string fileName)
    {
        Image imageToConvert = Image.FromFile(fileName);

        using var ms = new MemoryStream();
        imageToConvert.Save(ms, format: imageToConvert.RawFormat);
        return ms.ToArray();
    }

    public static async Task<int> GetIdByFullName(string fullName , string? faculty)
    {
        string firstName = fullName.Split(' ')[0];
        string Name = fullName.Split(' ')[1];
        string lastName = fullName.Split(' ')[2];

        string sql = " select first 1 s.id " +
            " from stud_gruppa sg " +
            " inner join gruppa gr on gr.id = sg.grup_id " +
            " inner join student s on student.id = sg.stud_id " +
            " inner join specialnost spec on specialnost.id = gr.spec_id " +
            " inner join fakultet fak on fakultet.id = specialnost.fak_id " +
            $" where s.famil = '{firstName}' and s.name = '{Name}' and s.otch = '{lastName}' and fak.nick = '{faculty}' and gr.is_vip = 'F'";

        using FbConnection connection = new("database=192.168.250.72:Cont;user=sysdba;password=Vtlysq~Bcgjkby2020;Charset=win1251;");
        await connection.OpenAsync();
        using FbCommand command = new(sql, connection);
        FbDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            if (reader.HasRows)
                return reader.GetInt32(0);
        }
        return 0;
    }

    public static async Task UpdateImage(byte[] picbyte, int id)
    {

        using FbConnection connection = new(Connection);
        await connection.OpenAsync();
        using FbCommand command = new("UPDATE STUDENT SET FOTO = @photo where id = @id", connection);

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "UPDATE STUDENT SET FOTO = @photo where id = @id";
        cmd.Parameters.AddWithValue("@photo", picbyte);
        cmd.Parameters.AddWithValue("@id", id);
        _ = await cmd.ExecuteNonQueryAsync();

    }

}

