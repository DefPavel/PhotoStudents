Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding.GetEncoding(1252);
string Path = "D:\\TEST-IMAGE\\";
Console.WriteLine("Введите Ник Института/Факультета:");
string? Faculty = Console.ReadLine();

if(!File.Exists(Path))
{
    IEnumerable<string> allfiles = Directory.EnumerateFiles(Path);

    foreach (string filename in allfiles)
    {
        // Обрезаем путь и берем только файл
        string file = filename.Split(Path)[1];
        // Обрезаем формат файла
        string fullName = file.Split('.')[0];

        if(!string.IsNullOrEmpty(Faculty))
        {
            // поиск человека (WTF Почему-то он не захотел работать асинхронно)
            int IdPerson = await ServiceFireBird.GetIdByFullName(fullName, Faculty);

            if (IdPerson != 0)
            {
                // Конвертируем в массив байтов
                byte[] photo = await Task.Run(() => ServiceFireBird.GetBytes(filename));
                // Загружаем на старую базу
                await ServiceFireBird.UpdateImage(photo, IdPerson);
            }
        }  
    }
    Console.WriteLine("Завершено!");
    Console.ReadKey();
}
else
{
    Console.WriteLine("Директория не найдена");
}

