namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.Factory
{
    public interface IProdukcjaEwidencjaSQLStateFactory
    {
        IProdukcjaEwidencjaSQLState PobierzStan(int zaznaczonyTabItem);
    }
}