var importer = new Agile.EmailImporter.Importer();
var emails = importer.ImportFromFile("emails.txt");
foreach(var email in emails) Console.WriteLine(email);