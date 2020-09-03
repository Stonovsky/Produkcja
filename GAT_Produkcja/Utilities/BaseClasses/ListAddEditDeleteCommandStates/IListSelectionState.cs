namespace GAT_Produkcja.Utilities.BaseClasses.ListAddEditDeleteCommandStates
{
    public interface IListSelectionState
    {
        string SelectEditButtonTitle { get; }

        void PreformWithSelection();
    }
}