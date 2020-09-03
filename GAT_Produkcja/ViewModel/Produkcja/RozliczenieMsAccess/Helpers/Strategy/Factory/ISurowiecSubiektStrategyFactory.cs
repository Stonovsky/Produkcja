namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory
{
    public interface ISurowiecSubiektStrategyFactory
    {
        /// <summary>
        /// Zwraca strategie dotyczaca pobierania surowca z SubiektGT - dynamicznie FIFO dla dodatnich ilosci (ZNazwy) lub statycznie z wykorzystaniem slownika
        /// </summary>
        /// <param name="enumSubiektFactory"></param>
        /// <returns></returns>
        ISurowiecSubiektStrategy PobierzStrategie(SurowiecSubiektFactoryEnum enumSubiektFactory);
    }
}