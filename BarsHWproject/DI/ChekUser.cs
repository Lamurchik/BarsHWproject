namespace BarsHWproject.DI
{
    public class ChekUser : IChekUser
    {
        public string? CheckUser(string login, string password)
        {
            if (login == "login" && password == "123") // услованая проверка тут могут быть даные из бд
                return "Admin";
            else if (login == "log" && password == "1234")
                return "User";
            else return null;
        }
    }
}
