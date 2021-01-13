namespace PhoneStation.PhoneNumber
{
    public interface IPhoneNumber
    {
        string Number { get; }
        string UserName { get; }
        double Money { get; }
        void ChangeBalance(double money);
    }
}
