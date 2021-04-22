using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.ViewModels.Base;

namespace RecompildPOS.ViewModels.Products.GenerateCode
{
    public class GenerateCodeViewModel : BaseViewModel
    {
        private string _codeValue;
        public string CodeValue
        {
            get { return _codeValue; }
            set
            {
                _codeValue = value; 
                OnPropertyChanged(nameof(CodeValue));
            }
        }

     
        private Enum _selectedCode;
        public Enum SelectedCode
        {
            get { return _selectedCode; }
            set
            {
                _selectedCode = value; 
                OnPropertyChanged(nameof(SelectedCode));
            }
        }


    }
}
