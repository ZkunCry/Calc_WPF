using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculatorTRPO
{
    class CalcViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private static CalcModel _calcModel;
        private static Parser _parser = new Parser();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly IMemory _writer;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        CommandBinding commandBinding = new CommandBinding();

        
        public string ParseStr
        {
            get => _calcModel.StringParse;
            set
            {
                if (_calcModel.StringParse != value)
                {
                    _calcModel.StringParse = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ParseStr)));
                    ClearErrors(nameof(ParseStr));
                }
            }
        }
      
        public string MemoryNumber
        {
            get => _writer.ReadSomewhere();
            set
            {
                if (_writer.ReadSomewhere()!= value)
                {
                    _writer.WriteSomewhere(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemoryNumber)));
                }
            }
        } 
        private RelayCommand addNumbers;
        public RelayCommand AddNumber
        {
            get
            {
                return addNumbers ??
                  (addNumbers = new RelayCommand(obj =>
                  {
                      string number = obj as string;

                      if (ParseStr == "Error")
                          ClearOut.Execute(null);
                      if (ParseStr != "0")
                          ParseStr += number;
                      else
                        ParseStr = number;
                  }));
            }
        }
        private RelayCommand deleteChar;
        public RelayCommand DeleteChar
        {
            get
            {
                return deleteChar ??
                  (deleteChar = new RelayCommand(obj =>
                  {
                      ParseStr = ParseStr.Pop_Back();
                  }));
            }
            
        }
        private RelayCommand clearOut;
        public RelayCommand ClearOut
        {
            get
            {
                return clearOut ??
                  (clearOut = new RelayCommand(obj =>
                  {
                      ParseStr = "0";
                      MemoryNumber = "0";
                      _writer.WriteSomewhere("0");
                  }));
            }

        }
        private RelayCommand addOper;
        public RelayCommand AddOper
        {
            get
            {
                return addOper ??
                  (addOper = new RelayCommand(obj =>
                  {
                      char[] pattern = { '+', '-', '/', '*' };
                      int index = ParseStr.LastIndexOfAny(pattern);
                      string operation = obj as string;
                      if (ParseStr == "Error")
                          ClearOut.Execute(null);

                      else if ("+-/*,".Contains(ParseStr[ParseStr.Length - 1].ToString()))
                      {
                          if (ParseStr.LastIndexOf(',') > 0 && operation == ",")
                              return;
                          ParseStr = ParseStr.Pop_Back();
                          ParseStr += operation;
                          return;
                      }
                      else if (index > 0 && operation == "," && ParseStr.IndexOf(',', index) != -1)
                      {
                          return;
                      }
                      else if (index < 0 && operation =="," && ParseStr.Contains(','))
                          return;
                      else
                          ParseStr += operation;
                  }));
            }
        }
        private RelayCommand startParsing;
        public RelayCommand StartParsing
        {
            get
            {
                return startParsing ??
                  (startParsing = new RelayCommand(obj =>
                  {
                      string result = _parser.StartParsing(ParseStr);
                      ValidateResultString(result);
                      var hasError = GetErrors(nameof(ParseStr));
                      if(hasError ==null)
                            ParseStr = _parser.StartParsing(ParseStr);
                  },(obj)=>!double.TryParse(ParseStr,out double result)));
            }
        }
        private RelayCommand saveNumberToMemory;
        public RelayCommand SaveNumberToMemory
        {
            get
            {
                return saveNumberToMemory ??
                  (saveNumberToMemory = new RelayCommand(obj =>
                  {
                      string option = obj as string;
                      MemoryNumber =_parser.StartParsing(ParseStr);
                      MemoryNumber = MemoryNumber.Contains('-') ? MemoryNumber.Remove(0, 1):MemoryNumber;
                      if (option == "M-" && !MemoryNumber.Contains("-"))
                      {
                          MemoryNumber = MemoryNumber.Insert(0, "-");
                      }
                      _writer.WriteSomewhere(MemoryNumber);

                  }));
            }
        }
        private RelayCommand readNumberFromMemory;
        public RelayCommand ReadNumberFromMemory
        {
            get
            {
                return readNumberFromMemory ??
                  (readNumberFromMemory = new RelayCommand(obj =>
                  {
                      char[] pattern = { '+', '-', '/', '*' };
                      int index = ParseStr.LastIndexOfAny(pattern);
                  
                      if (MemoryNumber is null)
                      {
                          MemoryNumber = "0";
                      }
                      else if (index >-1)
                      {

                          if (MemoryNumber.Contains('-') || char.IsDigit(ParseStr.Last()))
                          {
                              if (index + 1 != ParseStr.Length && MemoryNumber.Contains('-'))
                              {
                                  if (ParseStr[index + 1] == '-' || pattern.Contains(ParseStr[index]))
                                      ParseStr = ParseStr.Remove(index) + MemoryNumber;
                                  else
                                      ParseStr += MemoryNumber;
                              }
                              else if(index+1!=ParseStr.Length)
                              {
                                  ParseStr = ParseStr.Remove(index+1) + MemoryNumber;
                              }
                              else
                                  ParseStr += MemoryNumber;
                          }
                          else
                            ParseStr += MemoryNumber;
                          
                      }

                      else
                          ParseStr = MemoryNumber;
                  }, (obj)=> checkEndParseStr(ParseStr)));
            }
        }

        bool checkEndParseStr(string str)
        {
            if (str == null)
                return false;
            if(str.Last() == ')')
                return false;
            return true;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
             _errorsByPropertyName[propertyName] : null;
        }

        private RelayCommand clearNumberFromMemory;
        public RelayCommand ClearNumberFromMemory
        {
            get
            {
                return clearNumberFromMemory ??
                  (clearNumberFromMemory = new RelayCommand(obj =>
                  {
                      _writer.WriteSomewhere("0");
                  }));
            }
        }
        private void ValidateResultString(string parseString)
        {
            ClearErrors(nameof(ParseStr));
            if (parseString == "Error")
                AddError(nameof(ParseStr), "The expression cannot be parsed");
        }
        public bool HasErrors => _errorsByPropertyName.Any();

        public CalcViewModel(IMemory writer)
        {
            _writer = writer;
            _calcModel = new CalcModel();
            ParseStr = "0";
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();
            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
    }   
    
}
