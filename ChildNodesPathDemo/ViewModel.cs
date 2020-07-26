using DevExpress.RichEdit.Export;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

/*
     * Форматирование выбранной части Ctr+E+F, форматирование всего Ctr+E+D
     * Удалить строку Ctr+Shift+L, вырезать строку Ctrl+L поднять/опустить строку Alt+стрелка вверх/вниз
     * Добавить строку сверху Ctrl+Enter, добавить строку снизу Ctrl+Shift+Enter
     * Транспонировать строку Shift+ALT+T
     * Буфер обмена Ctr+Shift+V
     * Закоментировать выбранные линии Ctr+E,C;  раскомментировать Ctr+E,U
     * Окружить выделенный текст  Ctr+K,S
     * Дублировать строку Ctr+E,V вставляет выше
     * Быстрые действия Ctr+.
     * Несколько точек вставки Ctr+ALT+нажатие
     * Выбор блока ALT и тащим мышь или SHIFT+ALT+стрелки

     Отладка
     * В режиме отладки: Watch Ctr+D,W QuickWatch Ctr+D,Q
     * Удалить все точки останова Ctrl+Shift+F9

     Помощь
     * Помощник Ctr+Space, Ctr+J
     * Показ всех снипетов Ctr+K,X

     Навигация по коду:
     * Перейти к определению класса/метода/свойства F12
     * Перейти к номеру строки Ctr+G
     * Перейти к последнему изменению Ctrl+Shift+Backspace
     * Начало/окончание блоков {}, комментирования, или #region Ctr+]
     * Свернуть/развернуть блок Ctr+M,M
     * Свернуть/развернуть ВСЕ блоки Ctr+M,O
      
     Рефакторинг:
     * Ctrl+R, Ctrl+M(комбинация) — выделение метода
     * Ctrl+R, Ctrl+E(комбинация) — инкапсуляция свойства
     * Ctrl+R, Ctrl+I(комбинация) — выделение интерфейса
     * Ctrl+R, Ctrl+V(комбинация) — удаление параметра
     * Ctrl+R, Ctrl+O(комбинация) — изменить порядок параметров

 */

/* МОДИФИКАТОРЫ ДОСТУПА
 
    * public: публичный, общедоступный класс или член класса. Такой член класса доступен из любого места в коде, а также из других программ и сборок.
     
    * private: закрытый класс или член класса. Представляет полную противоположность модификатору public. 
               Такой закрытый класс или член класса доступен только из кода в том же классе или контексте.

    * protected: такой член класса доступен из любого места в текущем классе или в производных классах. 
    *            При этом производные классы могут располагаться в других сборках.
                
    * internal: класс и члены класса с подобным модификатором доступны из любого места кода в той же сборке, 
    *           однако он недоступен для других программ и сборок (как в случае с модификатором public).
               
    * protected internal: совмещает функционал двух модификаторов. 
    *                     Классы и члены класса с таким модификатором доступны из текущей сборки и из производных классов.
                         
    * private protected: такой член класса доступен из любого места в текущем классе или в производных классах, которые определены в той же сборке.                    
          
 */



namespace ChildNodesPathDemo
{

    public abstract class EplanNodeBase
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                GetProductGroupNameAndImageId(ParentNode, value);
                /*
                switch (value)
                {
                    //case "1":
                    //    _Name = "Электротехника";
                    //    _Image = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
                    //    OriginName = value;

                    //    break;

                    case "5":
                        _Name = "Преобразователи";
                        _Image = new BitmapImage(new Uri("/Resources/5_Преобразователи.ico", UriKind.Relative));
                        OriginName = value;

                        break;

                    case "6":
                        _Name = "Защитные устройства";
                        _Image = new BitmapImage(new Uri("/Resources/6_Защитные устройства.ico", UriKind.Relative));
                        OriginName = value;
                        break;

                    case "12":
                        _Name = "Сенсорная техника, выключатель и кнопочный переключатель";
                        _Image = new BitmapImage(new Uri("/Resources/12_Сенсорная техника, выключатель и кнопочный переключатель.ico", UriKind.Relative));
                        OriginName = value;
                        break;
                    default:
                        //убрираем ??_??@ и ru_RU@ путем проверки 3го и 6го символа в строках где это есть, и оставляем имя с 6го символа
                        if (value != null && value.Length > 6 && value[2] == '_' && value[5] == '@')
                        {
                            _Name = value.Substring(6);
                        }
                        else
                        {
                            _Name = value;
                        }
                        OriginName = value;
                        //_Image = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
                        _Image = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
                        
                        _ = ParentNode;
                        break;
                }
               */ 
            }
        }

        public string OriginName { get; private set; }
        public string Note { get; set; }
        private static int Id { get; set; }
        public int NodeId { get; private set; }
        public override string ToString() { return Name; }
        //private BitmapImage _Image;
        //public BitmapImage Image { get { return _Image; } }
         public BitmapImage Image { get; private set; }

        // private static BitmapImage _ImageId;
        public EplanNode ParentNode { get; set; }


        private void GetProductGroupNameAndImageId(in EplanNode ParentNode, string Value/*, ref string Name, ref BitmapImage Image */)
        {
            //Если это узел root
            if (ParentNode == null)
            {
                Image = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
                _Name = Value;
                OriginName = Value;
                return;
            }
            //Если у пришедшего узла родительским узлом являеттся узел root
            if (ParentNode?.NodeId == 0)
            {
                switch (Value)
                {
                    case "1":
                        _Name = "Электротехника";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
                        break;

                    case "2":
                        _Name = "Fluid-Техника";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
                        break;

                    case "3":
                        _Name = "Механика";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
                        break;

                    default:
                        _Name = "??????????????";
                        OriginName = Value;
                        break;
                }
            }
            else // иначе, если у пришедшего узла родительским является любой другой, то расшифруем их названия и выставим картинку
            {
                switch (Value)
                {


                    case "2":
                        _Name = "Реле,контакторы";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/2_Реле,контакторы.ico", UriKind.Relative));
                        break;

                    case "3":
                        _Name = "Клеммы";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/3_Клеммы.ico", UriKind.Relative));
                        break;

                    case "5":
                        _Name = "Преобразователи";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/5_Преобразователи.ico", UriKind.Relative));
                        break;

                    case "6":
                        _Name = "Защитные устройства";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/6_Защитные устройства.ico", UriKind.Relative));
                        break;

                    case "8":
                        _Name = "Сигнальные устройства";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/8(115)_Сигнальные устройства.ico", UriKind.Relative));
                        break;

                    case "10":
                        _Name = "Измерительные устройства, контрольное оборудование";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/10_Измерительные устройства, контрольное оборудование.ico", UriKind.Relative));
                        break;

                    case "12":
                        _Name = "Сенсорная техника, выключатель и кнопочный переключатель";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/12_Сенсорная техника, выключатель и кнопочный переключатель.ico", UriKind.Relative));
                        break;

                    case "13":
                        _Name = "Трансформаторы";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/13_Трансформаторы.ico", UriKind.Relative));
                        break;

                    case "17":
                        _Name = "Разное";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/17(47)_Разное(Принадлежности).ico", UriKind.Relative));
                        break;

                    case "20":
                        _Name = "Источник напряжения и генератор";
                        OriginName = Value;
                        Image = new BitmapImage(new Uri("/Resources/20_Источник напряжения и генератор.ico", UriKind.Relative));
                        break;

                    default:
                        //убрираем ??_??@ и ru_RU@ путем проверки 3го и 6го символа в строках где это есть, и оставляем имя с 6го символа
                        if (Value != null && Value.Length > 6 && Value[2] == '_' && Value[5] == '@')
                        {
                            _Name = Value.Substring(6);
                        }
                        else
                        {
                            _Name = Value;
                        }
                        // если пришел узел у которого нечего расшифровывать, то запомним его оригинальное имя из таблицы БД 
                        // и запомним картинку, которая была у родителя
                        OriginName = Value;
                        Image = ParentNode?.Image;
                        break;
                }
            }
        }

        public EplanNodeBase()
        {

            NodeId = Id++;
        }

    }

    public class EplanNode : EplanNodeBase
    {

        public ObservableCollection<EplanNode> SubNode { get; set; }



    }


    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EplanNode> DataItems { get; set; }

        private string _Test = "ПРИВЯЗКА";
        public string Test
        {
            get { return _Test; }
            set
            {
                if (Equals(_Test, value)) return;
                _Test = value;
                OnPropertyCnaged();
            }
        }



        #region SelectedNode1 : TreeListNode - выделенный узел первого деерева
        private TreeListNode _SelectedNode1;
        public TreeListNode SelectedNode1
        {
            get { return _SelectedNode1; }
            set
            {
                if (Equals(_SelectedNode1, value)) return;
                _SelectedNode1 = value;
                OnPropertyCnaged();
            }
        }
        #endregion

        #region SelectedNode2 : TreeListNode - выделенный узел второго деерева
        private TreeListNode _SelectedNode2;
        public TreeListNode SelectedNode2
        {
            get { return _SelectedNode2; }
            set
            {
                if (Equals(_SelectedNode2, value)) return;
                _SelectedNode2 = value;
                OnPropertyCnaged();
            }
        }
        #endregion





        public ViewModel()
        {
            DataItems = InitData();


        }

        public void Foo()
        {
            //нашли объект выделенного узла. Распрямили все дерево и в нем нашли первый объект у которого Id совпадают
            EplanNode result2 = DataItems.Flatten(i => i.SubNode).
                                          FirstOrDefault(i => (/*i.Name == ((EplanNode)SelectedNode2?.Content)?.Name && */
                                                                 i.NodeId == ((EplanNode)SelectedNode2?.Content)?.NodeId));

            if (SelectedNode1 != null)
            {
                SelectedNode2.IsExpanded = true;
            }

            // у выделеннго узла есть дети, то кладем в ребенка узел, по которому кликнули в первом дереве    
            if (result2.SubNode != null)
            {
               // ((EplanNode)SelectedNode1?.Content).Image = ((EplanNode)SelectedNode2?.Content).Image;
                result2.SubNode.Add((EplanNode)SelectedNode1?.Content);
            }
            else MessageBox.Show("Сюда класть нихуя нельзя!");

        }



        public string Foo2()
        {

            EplanNode result2 = DataItems.Flatten(i => i.SubNode).
                                          FirstOrDefault(i => i.NodeId == ((EplanNode)SelectedNode2?.Content)?.NodeId);

            return result2.NodeId.ToString();

            // MessageBox.Show(result2.NodeId.ToString());

        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyCnaged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }



        private ObservableCollection<EplanNode> InitData()
        {
            ObservableCollection<EplanNode> RootNode = new ObservableCollection<EplanNode> { new EplanNode() { ParentNode = null, Name = "Изделие", SubNode = new ObservableCollection<EplanNode>() } };

            InitProjectData(RootNode);
            return RootNode;
        }

        //ищем в коллекции индекс по свойству Name, чтобы внутри него добавить SubNode
        private static int FindIndexInCollection(ObservableCollection<EplanNode> Collection, string find_string)
        {
            //начинаем с конца, скорее новый элемент будет в конце дерева
            for (int i = Collection.Count - 1; i >= 0; --i)
            {
                if (Collection[i].OriginName == find_string)
                {
                    return i;
                }
            }
            return -15;//magic number так как индексы не могут быть отрицательными, и если такого элемента еще нет в коллекции, возвращаем число любое меньше нуля (обычно -1 (типа код ошибки))
        }

        void InitProjectData(ObservableCollection<EplanNode> RootNode)
        {
            #region Подключение к БД, SQL запрос и сохранение в dataset

            // либо создадим эту строку сами напрямую
            string connectionString = @"Provider=Microsoft.JET.OLEDB.4.0; Data Source=C:\Users\bugrov\Desktop\sample_\ChildNodesPathDemo\db\База_Изделий.mdb";
            //string connectionString = @"Provider=Microsoft.JET.OLEDB.4.0; Data Source=D:\Личное\Проект С#\sample_\ChildNodesPathDemo\db\База_Изделий.mdb";
            //@"Provider=Microsoft.JET.OLEDB.4.0; Data Source=..\..\db\ESS_part005_ONLY_ABB.mdb";
            //@"Provider=Microsoft.JET.OLEDB.4.0; Data Source=..\..\db\ESS_part002.mdb";
            //@"Provider=Microsoft.JET.OLEDB.4.0; Data Source=F:\Project KP\ESS_part002.mdb";
            //База_Изделий.mdb


            //Создадим объект класса DataSet, в который происходит копирование из OleDbDataAdapter
            DataSet dataSet = new DataSet();

            //само подключение и запрос
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string SQLQuery = "SELECT * FROM tblPart order by supplier";

                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        // MessageBox.Show("Подключились к базе. Путь и имя базы " + connection.DataSource);
                        // MessageBox.Show(connection.Provider);


                        //создадим объект-команду, которая отсылает запрос на сревер (в БД) и в конструкто сразу передадим строку запроса и соединение
                        using (OleDbCommand command = new OleDbCommand(SQLQuery, connection))
                        {
                            //Отправляем команду на сервер и запоминаем результат в объект OleDbDataAdapter
                            OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                            //Создадим объект класса DataSet, в который происходит копирование из OleDbDataAdapter
                            // DataSet dataSet = new DataSet();

                            //Заполняем (копируем) данные
                            adapter.Fill(dataSet);
                        }

                        //закрываем соединение
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

            // из датасет получим строки и преобразуем их в IEnumerable
            IEnumerable<DataRow> seq = dataSet.Tables[0].AsEnumerable();

            #endregion

            #region Просто код для экспериментов
            /*
             try
             {
                 List<string> sup = seq.Distinct(DataRowComparer.Default).Select(s => s.Field<string>("supplier")).ToList();
                 // List<short> topgr = seq.Distinct(DataRowComparer.Default).Select(s => s.Field<short>("productgroup")).ToList();
                 List<short> topgr = seq.Where(s => s.Field<string>("supplier") == sup[2]).Select(s => s.Field<short>("productgroup")).Distinct().ToList();

                 IEnumerable<DataRow> topgr2 = seq.Distinct(DataRowComparer.Default);
                 // IEnumerable<string> topgr22 = seq.d; 
            */


            /*
                            foreach (var item in topgr2)
                            {
                                MessageBox.Show($"{item.Field<short>("productgroup")}");
                            }

                            foreach (var item in sup)
                            {
                                MessageBox.Show($"{item}");
                            }

                            foreach (var item in topgr)
                            {
                                MessageBox.Show($"{item}");
                            }

                            MessageBox.Show($"Топгруп {topgr[0]}");
            */

            /*
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

             */
            #endregion


            #region ЗАПИСЬ в ФАЙЛ ДЛЯ ПРОВЕРКИ
            /*
            StreamWriter sw = File.CreateText(@"C:\Users\bugrov\Desktop\qwe.txt");
            try
            {
                foreach (Placement3D SelectedPlacements3D in SO)
                {
                    PlacementsList.Add(SelectedPlacements3D);
                    sw.WriteLine(SelectedPlacements3D.Properties.FUNC_FULLDEVICETAG.ToString() +
                        "        Имя пространства  " + SelectedPlacements3D.InstallationSpace.VisibleName);

                    //MessageBox.Show("Имя выделенного пространства метод Selection: " + SelectedSpaces.VisibleName);
                }
            }
            catch
            {
                MessageBox.Show("Что-то сука пошло не так");
            }
            finally
            {
                MessageBox.Show("Запись окончена");
                sw.Close();
            }
            */
            #endregion


            #region MyRegion

            // List<short> ProductTopGroupName = seq.Select(s => s.Field<short>("producttopgroup")).Distinct().ToList();
            // List<string> SupplierName;
            // List<short> ProductGroupName;
            // List<string> TypeNrName;
            // List<string> Description1Name;
            // List<string> Description2Name;
            // List<string> Description3Name;
            // List<string> PartNrName; 
            #endregion

            //достаем все значения всех нужных нам столбцов, так как структура записей в бд одинаковая, то кол-во эл-в везде одинаково
            List<short> ProductTopGroupName = seq.Select(s => s.Field<short>("producttopgroup")).ToList();
            List<string> SupplierName = seq.Select(s => s.Field<string>("supplier")).ToList();
            List<short> ProductGroupName = seq.Select(s => s.Field<short>("productgroup")).ToList();
            List<string> TypeNrName = seq.Select(s => s.Field<string>("typenr")).ToList();
            List<string> Description1Name = seq.Select(s => s.Field<string>("description1")).ToList();
            List<string> Description2Name = seq.Select(s => s.Field<string>("description2")).ToList();
            List<string> Description3Name = seq.Select(s => s.Field<string>("description3")).ToList();
            List<string> PartNrName = seq.Select(s => s.Field<string>("partnr")).ToList();
            List<string> NoteName = seq.Select(s => s.Field<string>("note")).ToList();

            ObservableCollection<EplanNode> ProductTopGroupNode = RootNode[0].SubNode;


            #region MyRegion

            // ObservableCollection<EplanNode> SupplierNode = new ObservableCollection<EplanNode>();
            // ObservableCollection<EplanNode> ProductGroupNode = new ObservableCollection<EplanNode>();
            // ObservableCollection<EplanNode> TypeNrNode = new ObservableCollection<EplanNode>();
            // ObservableCollection<EplanNode> Description1Node = new ObservableCollection<EplanNode>();
            // ObservableCollection<EplanNode> Description2Node = new ObservableCollection<EplanNode>();
            // ObservableCollection<EplanNode> Description3Node = new ObservableCollection<EplanNode>();
            // ObservableCollection<EplanNode> PartNrNode = new ObservableCollection<EplanNode>(); 
            #endregion

            //так как количество строчек во всех столбцах одинаково, идем по любому из них, в данном случае по первому, идем по каждой строчке и заполняем дерево.  
            // То есть по сути формируем слои дерева 
            for (int i = 0; i < ProductTopGroupName.Count; ++i)
            {
                string string_ProductTopGroupName = ProductTopGroupName[i].ToString();
                int index;
                int ParentIndex;
                //ищем элемент по имени, проверяя его уникальность
                index = FindIndexInCollection(ProductTopGroupNode, string_ProductTopGroupName);
                //если не нашли эл-т с таким именем, то добавляем его в конец и запоминаем индекс добавленного
                if (index < 0)
                {
                    ProductTopGroupNode.Add(new EplanNode() { ParentNode = RootNode[0], Name = string_ProductTopGroupName, SubNode = new ObservableCollection<EplanNode>() });
                    index = ProductTopGroupNode.Count - 1; // индекс последнего добавленного элемента. Т.к. метод Add добавляет в конец коллекции
                    ParentIndex = index;
                }

                //переходим вниз по дереву к следующей группе элементов в ту ветвь, в которую зашли выше
                ParentIndex = index;// запомнили вышестоящий(родительский индекс)
                ObservableCollection<EplanNode> SupplierNode = ProductTopGroupNode[index].SubNode;
                index = FindIndexInCollection(SupplierNode, SupplierName[i]);
                if (index < 0)
                {
                    SupplierNode.Add(new EplanNode() { ParentNode = ProductTopGroupNode[ParentIndex], Name = SupplierName[i], SubNode = new ObservableCollection<EplanNode>() });
                    index = SupplierNode.Count - 1;
                    ParentIndex = index;
                }

                ParentIndex = index;
                ObservableCollection<EplanNode> ProductGroupNode = SupplierNode[index].SubNode;
                string string_ProductGroupName = ProductGroupName[i].ToString();
                index = FindIndexInCollection(ProductGroupNode, string_ProductGroupName);
                if (index < 0)
                {
                    ProductGroupNode.Add(new EplanNode() { ParentNode = SupplierNode[ParentIndex], Name = string_ProductGroupName, SubNode = new ObservableCollection<EplanNode>() });
                    index = ProductGroupNode.Count - 1;
                    ParentIndex = index;

                }

                ParentIndex = index;
                ObservableCollection<EplanNode> TypeNrNode = ProductGroupNode[index].SubNode;
                index = FindIndexInCollection(TypeNrNode, TypeNrName[i]);
                if (index < 0)
                {
                    TypeNrNode.Add(new EplanNode() { ParentNode = ProductGroupNode[ParentIndex], Name = TypeNrName[i], SubNode = new ObservableCollection<EplanNode>() });
                    index = TypeNrNode.Count - 1;
                    ParentIndex = index;

                }

                ParentIndex = index;
                ObservableCollection<EplanNode> Description1Node = TypeNrNode[index].SubNode;
                index = FindIndexInCollection(Description1Node, Description1Name[i]);
                if (index < 0)
                {
                    Description1Node.Add(new EplanNode() { ParentNode = TypeNrNode[ParentIndex], Name = Description1Name[i], SubNode = new ObservableCollection<EplanNode>() });
                    index = Description1Node.Count - 1;
                    ParentIndex = index;

                }

                ParentIndex = index;
                ObservableCollection<EplanNode> Description2Node = Description1Node[index].SubNode;
                index = FindIndexInCollection(Description2Node, Description2Name[i]);
                if (index < 0)
                {
                    Description2Node.Add(new EplanNode() { ParentNode = Description1Node[ParentIndex], Name = Description2Name[i], SubNode = new ObservableCollection<EplanNode>() });
                    index = Description2Node.Count - 1;
                    ParentIndex = index;

                }
                ParentIndex = index;
                ObservableCollection<EplanNode> Description3Node = Description2Node[index].SubNode;
                index = FindIndexInCollection(Description3Node, Description3Name[i]);
                if (index < 0)
                {
                    Description3Node.Add(new EplanNode() { ParentNode = Description2Node[ParentIndex], Name = Description3Name[i], SubNode = new ObservableCollection<EplanNode>() });
                    index = Description3Node.Count - 1;
                    ParentIndex = index;

                }

                ParentIndex = index;
                ObservableCollection<EplanNode> PartNrNode = Description3Node[index].SubNode;
                index = FindIndexInCollection(PartNrNode, PartNrName[i]);
                if (index < 0)
                {
                    PartNrNode.Add(new EplanNode() { ParentNode = Description3Node[ParentIndex], Name = PartNrName[i], Note = NoteName[i] });
                    index = PartNrNode.Count - 1;
                }
            }

            //переименовываем элементы из столбца и сортируем по алфавиту названия из столбца supplier
           /* foreach (var node in ProductTopGroupNode)
            {
                if (node.OriginName == "1")
                {
                    node.Name = "Электротехника";
                }
                else if (node.OriginName == "2")
                {
                    node.Name = "Fluid - техника";
                }
                else if (node.OriginName == "3")
                {
                    node.Name = "Механика";
                }
           
                //          node.SubNode = new ObservableCollection<EplanNode>(node.SubNode.OrderBy(i => i.Name));
            }*/

            #region Старые циклы)
            // for (int i = 0; i < ProductTopGroupName.Count; i++)
            // {
            //     ProductTopGroupNode.Add(new EplanNode() { Name = ProductTopGroupName[i].ToString(), SubNode = new ObservableCollection<EplanNodeBase>() });
            //     SupplierName = seq.Where(s => s.Field<short>("producttopgroup") == ProductTopGroupName[i]).Select(s => s.Field<string>("supplier")).Distinct().ToList();

            //     for (int index = 0; index < SupplierName.Count; index++)
            //     {
            //         SupplierNode.Add(new EplanNode() { Name = SupplierName[index], SubNode = new ObservableCollection<EplanNodeBase>() });
            //         var jj = SupplierNode[SupplierNode.IndexOf(SupplierNode.Last(x => x.Name == SupplierName[index]))];
            //         ProductTopGroupNode[i].SubNode.Add(jj);
            //         // ProductTopGroupNode[i].Tasks.Add(SupplierNode[index]);
            //         ProductGroupName = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
            //                                           (s.Field<string>("supplier") == SupplierName[index])).Select(s => s.Field<short>("productgroup")).Distinct().ToList();

            //         for (int k = 0; k < ProductGroupName.Count; k++)
            //         {
            //             ProductGroupNode.Add(new EplanNode() { Name = ProductGroupName[k].ToString(), SubNode = new ObservableCollection<EplanNodeBase>() });
            //             var kk = ProductGroupNode[ProductGroupNode.IndexOf(ProductGroupNode.Last(x => x.OriginName == ProductGroupName[k].ToString()))];
            //             jj.SubNode.Add(kk);
            //             //SupplierNode[index].Tasks.Add(ProductGroupNode[ProductGroupNode.IndexOf(ProductGroupNode.First(x => x.Name == ProductGroupName[k].ToString()))]);

            //             TypeNrName = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
            //                                         (s.Field<string>("supplier") == SupplierName[index]) &&
            //                                         (s.Field<short>("productgroup") == ProductGroupName[k])).Select(s => s.Field<string>("typenr")).Distinct().ToList();

            //             for (int l = 0; l < TypeNrName.Count; l++)
            //             {
            //                 TypeNrNode.Add(new EplanNode() { Name = TypeNrName[l], SubNode = new ObservableCollection<EplanNodeBase>() });
            //                 var ll = TypeNrNode[TypeNrNode.IndexOf(TypeNrNode.Last(x => x.Name == TypeNrName[l]))];
            //                 //ProductGroupNode[kk].Tasks.Add(TypeNrNode[TypeNrNode.IndexOf(TypeNrNode.First(x => x.Name == TypeNrName[l]))]);
            //                 kk.SubNode.Add(ll);

            //                 Description1Name = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
            //                                                   (s.Field<string>("supplier") == SupplierName[index]) &&
            //                                                   (s.Field<short>("productgroup") == ProductGroupName[k]) &&
            //                                                   (s.Field<string>("typenr") == TypeNrName[l])).Select(s => s.Field<string>("description1")).Distinct().ToList();

            //                 for (int m = 0; m < Description1Name.Count; m++)
            //                 {
            //                     Description1Node.Add(new EplanNode() { Name = Description1Name[m], SubNode = new ObservableCollection<EplanNodeBase>() });
            //                     var mm = Description1Node[Description1Node.IndexOf(Description1Node.Last(x => x.Name == Description1Name[m]))];
            //                     //TypeNrNode[l].Tasks.Add(Description3Node[Description3Node.IndexOf(Description3Node.First(x => x.Name == Description3Name[m]))]);
            //                     ll.SubNode.Add(mm);

            //                     Description2Name = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
            //                                                       (s.Field<string>("supplier") == SupplierName[index]) &&
            //                                                       (s.Field<short>("productgroup") == ProductGroupName[k]) &&
            //                                                       (s.Field<string>("typenr") == TypeNrName[l]) &&
            //                                                       (s.Field<string>("description1") == Description1Name[m])).Select(s => s.Field<string>("description2")).Distinct().ToList();

            //                     for (int n = 0; n < Description2Name.Count; n++)
            //                     {
            //                         Description2Node.Add(new EplanNode() { Name = Description2Name[n], SubNode = new ObservableCollection<EplanNodeBase>() });
            //                         var nn = Description2Node[Description2Node.IndexOf(Description2Node.Last(x => x.Name == Description2Name[n]))];
            //                         mm.SubNode.Add(nn);

            //                         //Description3Node[m].Tasks.Add(Description2Node[Description2Node.IndexOf(Description2Node.First(x => x.Name == Description2Name[n]))]);
            //                         Description3Name = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
            //                                                           (s.Field<string>("supplier") == SupplierName[index]) &&
            //                                                           (s.Field<short>("productgroup") == ProductGroupName[k]) &&
            //                                                           (s.Field<string>("typenr") == TypeNrName[l]) &&
            //                                                           (s.Field<string>("description1") == Description1Name[m]) &&
            //                                                           (s.Field<string>("description2") == Description2Name[n])).Select(s => s.Field<string>("description3")).Distinct().ToList();

            //                         for (int o = 0; o < Description3Name.Count; o++)
            //                         {
            //                             Description3Node.Add(new EplanNode() { Name = Description3Name[o], SubNode = new ObservableCollection<EplanNodeBase>() });
            //                             var oo = Description3Node[Description3Node.IndexOf(Description3Node.Last(x => x.Name == Description3Name[o]))];
            //                             nn.SubNode.Add(oo);

            //                             //Description2Node[n].Tasks.Add(Description1Node[Description1Node.IndexOf(Description1Node.First(x => x.Name == Description1Name[o]))]);
            //                             PartNrName = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
            //                                                               (s.Field<string>("supplier") == SupplierName[index]) &&
            //                                                               (s.Field<short>("productgroup") == ProductGroupName[k]) &&
            //                                                               (s.Field<string>("typenr") == TypeNrName[l]) &&
            //                                                               (s.Field<string>("description1") == Description1Name[m]) &&
            //                                                               (s.Field<string>("description2") == Description2Name[n]) &&
            //                                                               (s.Field<string>("description3") == Description3Name[o])).Select(s => s.Field<string>("partnr")).Distinct().ToList();

            //                             for (int p = 0; p < PartNrName.Count; p++)
            //                             {
            //                                 PartNrNode.Add(new EplanNode() { Name = PartNrName[p] });
            //                                 var pp = PartNrNode[PartNrNode.IndexOf(PartNrNode.Last(x => x.Name == PartNrName[p]))];
            //                                 oo.SubNode.Add(pp);

            //                                 // Description1Node[o].Tasks.Add(PartNrNode[PartNrNode.IndexOf(PartNrNode.First(x => x.Name == PartNrName[p]))]);



            //                             }
            //                         }
            //                     }


            //                 }


            //             }
            //         }


            //     }
            //     Project.SubNode.Add(ProductTopGroupNode[i]);

            // }
            #endregion
        }


    }


}

