using System.Text.RegularExpressions;
namespace Agile.EmailImporter
{
    public class Importer
    {
        public List<string> ImportFromFile(string filename)
        {
            var emails = new List<string>();
            var lines = File.ReadAllText(filename).Split(';');
            foreach (var line in lines)
            {
                var email = GetEmail(line);
                emails.Add(email);
                
            }
            return emails;
        }

        private string GetEmail(string line)
        {
            var email = extractEmails(line);
            if (email.Contains('@')) return email;
            return "!!!" + line;
        }

        private string extractEmails(string text)
        {
            Regex re = new Regex(@"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                  + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                  + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                  + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})");
            return re.Match(text).Value;
        }
    }
}