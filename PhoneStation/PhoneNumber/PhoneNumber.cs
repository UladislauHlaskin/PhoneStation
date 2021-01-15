namespace PhoneStation.PhoneNumber
{
    public class PhoneNumber : IPhoneNumber
    {
        public string Number { get; private set; }

        public string UserName { get; private set; }

        public double Money { get; private set; }

        public PhoneNumber(string number, string userName)
        {
            Number = number;
            UserName = userName;
        }

        public PhoneNumber(string number, string userName, double money) : this(number, userName)
        {
            Money = money;
        }

        public void ChangeBalance(double money)
        {
            Money += money;
        }

        public override string ToString()
        {
            return $"{UserName}, {Number}";
        }
    }
}
