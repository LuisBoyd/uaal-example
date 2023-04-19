﻿namespace UI.RecyclableScrollRect
{
    public interface IRecyclableScrollRectDataSource
    {
        int GetItemCount();
        void SetCell(ICell cell, int index);
    }
}