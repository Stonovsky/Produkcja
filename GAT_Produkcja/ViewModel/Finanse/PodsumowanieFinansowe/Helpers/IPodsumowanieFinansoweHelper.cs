namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public interface IPodsumowanieFinansoweHelper
    {
        IPodsumowanieZamowieniaOdKlientowHelper PodsumowanieZamowieniaOdKlientowHelper { get; }
        IPodsumowanieProdukcjaHelper PodsumowanieProdukcjaHelper { get; }
        IPodsumowanieSprzedazHelper PodsumowanieSprzedazHelper { get; }
        IPodsumowanieMagazynyHelper PodsumowanieMagazynyHelper { get; }
        IPodsumowanieNaleznosciIZobowiazaniaHelper PodsumowanieNaleznosciIZobowiazaniaHelper { get; }
        IPodsumowanieKontaBankoweHelper PodsumowanieKontBankowychHelper { get; }
    }
}