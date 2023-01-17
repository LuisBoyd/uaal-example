namespace UI.uGUI
{
    public interface IDataBindable<D,O>
    {
        //D - Data
        //O - object to bind it to

        void BindData(D data, O obj);
    }
}