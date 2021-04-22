using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RecompildPOS.Models.Selectable
{
    public class SelectableItem<T> : INotifyPropertyChanged
    {
        public T Item { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public SelectableItem(T value)
        {
            Item = value;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
