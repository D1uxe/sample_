using DevExpress.XtraRichEdit.Commands.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

/*
     * В режиме отладки: Watch Ctr+D,W QuickWatch Ctr+D,Q
     * Форматирование выбранной части Ctr+E+F, форматирование всего Ctr+E+D
     * Удалить строку Ctr+Shift+L, вырезать строку Ctrl+L поднять/опустить строку Alt+стрелка вверх/вниз
     * Добавить строку сверзу Ctrl+Enter, добавить строку снизу Ctrl+Shift+Enter
     * Транспонировать строку Shift+ALT+T
     * Буфер обмена Ctr+Shift+V
     * Свернуть/развернуть блок Ctr+M,M
     * Свернуть/развернуть ВСЕ блоки Ctr+M,O
     * Начало/окончание блоков {}, комментирования, или #region Ctr+]
     * Перейти к номеру строки Ctr+G
     * Перейти к определению класса/метода/свойства F12
     * Помощник Ctr+Space, Ctr+J
     * Показ всех снипетов Ctr+K,X
     * Закоментировать выбранные линии Ctr+E,C;  раскомментировать Ctr+E,U
     * Окружить выделенный текст  Ctr+K,S
     * Дублировать строку Ctr+E,V
     * Быстрые действия Ctr+.
     * Несколько точек вставки Ctr+ALT+нажатие
     * Выбор блока ALT и тащим мышь или SHIFT+ALT+стрелки
     * Удалить все точки останова Ctrl+Shift+F9
     * 
     */
namespace ChildNodesPathDemo
{

	public class BaseObject
	{
		private string _Name;
		public string Name
		{
			get
			{
				return _Name; 
			}
			set{
				switch (value)
				{
					case "5":
						_Name = "Преобразователи";
						StaticImage = new BitmapImage(new Uri("/Resources/5_Преобразователи.ico", UriKind.Relative));
						OriginName = value;
						break;

					case "6":
						_Name = "Защитные утсройства";
						StaticImage = new BitmapImage(new Uri("/Resources/6_Защитные устройства.ico", UriKind.Relative));
						OriginName = value;
						break;

					case "12":
						_Name = "Сенсорная техника, выключатель и кнопочный переключатель";
						StaticImage = new BitmapImage(new Uri("/Resources/12_Сенсорная техника, выключатель и кнопочный переключатель.ico", UriKind.Relative));
						OriginName = value;
						break;
					default:
						//убрираем ??_??@ и ru_RU@ путем проверки 3го и 6го символа в строках где это есть, и оставляем имя с 6го символа
						if (value != null && value.Length>6 && value[2] == '_' && value[5] == '@')
						{
							_Name = value.Substring(6);
						}
						else
						{
							_Name = value;
						}
						OriginName = value;
						StaticImage = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
						break;
				}
			}
		}

		public string OriginName { get; private set; }
		public string Executor { get; set; }

		public override string ToString() { return Name; }

		protected BitmapImage StaticImage;
		public BaseObject()
		{
			//StaticImage = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
		}
		public BitmapImage Image { get { return StaticImage; } }
	}

	public class ProjectObject : BaseObject
	{
		public ObservableCollection<ProjectObject> SubNode { get; set; }

	}


	public class ViewModel
	{
		public ObservableCollection<ProjectObject> DataItems { get; set; }

		public ViewModel()
		{
			DataItems = InitData();
		}

		private ObservableCollection<ProjectObject> InitData()
		{
			ObservableCollection<ProjectObject> projects = new ObservableCollection<ProjectObject>();
			//ProjectObject Project = new ProjectObject() { Name = "ProductTopGroup Электротехника", SubNode = new ObservableCollection<ProjectObject>() };

			InitProjectData(projects);


			//projects.Add(Project);


			return projects;
		}

		//ищем в коллекции индекс по строчке, чтобы внутри него добавить SubNode
		private static int find_index_in_collection(ObservableCollection<ProjectObject> collection, string find_string)
		{
			//начинаем с конца, скорее новый элемент будет в конце дерева
			for (int i = collection.Count - 1; i >= 0; --i)
			{
				if (collection[i].OriginName == find_string)
				{
					return i;
				}
			}
			return -15;//magic number так как индексы не могут быть отрицательными, и если такого элемента еще нет в коллекции, возвращаем число любое меньше нуля (обычно -1 (типа код ошибки))
		}

		void InitProjectData(ObservableCollection<ProjectObject> Project)
		{
			#region Подключение к БД, SQL запрос и сохранение в dataset

			// либо создадим эту строку сами напрямую
			string connectionString = @"Provider=Microsoft.JET.OLEDB.4.0; Data Source=C:\Users\bugrov\source\repos\WpfApp1\ESS_part002.mdb";
			//@"Provider=Microsoft.JET.OLEDB.4.0; Data Source=C:\Users\bugrov\source\repos\WpfApp1\ESS_part005_ONLY_ABB.mdb";
			//@"Provider=Microsoft.JET.OLEDB.4.0; Data Source=C:\Users\bugrov\source\repos\WpfApp1\ESS_part002.mdb";
			//@"Provider=Microsoft.JET.OLEDB.4.0; Data Source=F:\Project KP\ESS_part002.mdb";
			//База_Изделий.mdb


			//Создадим объект класса DataSet, в который происходит копирование из OleDbDataAdapter
			DataSet dataSet = new DataSet();

			//само подключение и запрос
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				string SQLQuery = "SELECT * FROM tblPart";

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


			
			// List<short> ProductTopGroupName = seq.Select(s => s.Field<short>("producttopgroup")).Distinct().ToList();
			// List<string> SupplierName;
			// List<short> ProductGroupName;
			// List<string> TypeNrName;
			// List<string> Description1Name;
			// List<string> Description2Name;
			// List<string> Description3Name;
			// List<string> PartNrName;

			//достаем все значения всех нужных нам столбцов, так как структура записей в бд одинаковая, то кол-во эл-в везде одинаково
			List<short> ProductTopGroupName = seq.Select(s => s.Field<short>("producttopgroup")).ToList();
			List<string> SupplierName = seq.Select(s => s.Field<string>("supplier")).ToList();
			List<short> ProductGroupName = seq.Select(s => s.Field<short>("productgroup")).ToList();
			List<string> TypeNrName = seq.Select(s => s.Field<string>("typenr")).ToList();
			List<string> Description1Name = seq.Select(s => s.Field<string>("description1")).ToList();
			List<string> Description2Name = seq.Select(s => s.Field<string>("description2")).ToList();
			List<string> Description3Name = seq.Select(s => s.Field<string>("description3")).ToList();
			List<string> PartNrName = seq.Select(s => s.Field<string>("partnr")).ToList();

		
			ObservableCollection<ProjectObject> ProductTopGroupNode = Project;
			// ObservableCollection<ProjectObject> SupplierNode = new ObservableCollection<ProjectObject>();
			// ObservableCollection<ProjectObject> ProductGroupNode = new ObservableCollection<ProjectObject>();
			// ObservableCollection<ProjectObject> TypeNrNode = new ObservableCollection<ProjectObject>();
			// ObservableCollection<ProjectObject> Description1Node = new ObservableCollection<ProjectObject>();
			// ObservableCollection<ProjectObject> Description2Node = new ObservableCollection<ProjectObject>();
			// ObservableCollection<ProjectObject> Description3Node = new ObservableCollection<ProjectObject>();
			// ObservableCollection<ProjectObject> PartNrNode = new ObservableCollection<ProjectObject>();

			//так как количество строчек во всех столбцах одинаково, идем по любому из них, в данном случае по первому, идем по каждой строчке и заполняем дерево.  
			// То есть по сути формируем слои дерева 
			for (int i = 0; i < ProductTopGroupName.Count; ++i)
			{
				string string_ProductTopGroupName = ProductTopGroupName[i].ToString();
				int j;
				//ищем элемент по имени, проверяя его уникальность
				j = find_index_in_collection(ProductTopGroupNode, string_ProductTopGroupName);
				//если не нашли эл-т с таким именем, то добавляем его в конец и запоминаем индекс добавленного
				if (j < 0)
				{
					ProductTopGroupNode.Add(new ProjectObject() { Name = string_ProductTopGroupName, SubNode = new ObservableCollection<ProjectObject>() });
					j = ProductTopGroupNode.Count - 1; // индекс последнего добавленного элемента. Т.к. метод Add добавляет в конец коллекции
				}
				//переходим вниз по дереву к следующей группе элементов в ту ветвь, в которую зашли выше
				ObservableCollection<ProjectObject> SupplierNode = ProductTopGroupNode[j].SubNode;
				j = find_index_in_collection(SupplierNode, SupplierName[i]);
				if (j < 0)
				{
					SupplierNode.Add(new ProjectObject() { Name = SupplierName[i], SubNode = new ObservableCollection<ProjectObject>() });
					j = SupplierNode.Count - 1;
				}

				ObservableCollection<ProjectObject> ProductGroupNode = SupplierNode[j].SubNode;
				string string_ProductGroupName = ProductGroupName[i].ToString();
				j = find_index_in_collection(ProductGroupNode, string_ProductGroupName);
				if (j < 0)
				{
					ProductGroupNode.Add(new ProjectObject() { Name = string_ProductGroupName, SubNode = new ObservableCollection<ProjectObject>() });
					j = ProductGroupNode.Count - 1;
				}

				ObservableCollection<ProjectObject> TypeNrNode = ProductGroupNode[j].SubNode;
				j = find_index_in_collection(TypeNrNode, TypeNrName[i]);
				if (j < 0)
				{
					TypeNrNode.Add(new ProjectObject() { Name = TypeNrName[i], SubNode = new ObservableCollection<ProjectObject>() });
					j = TypeNrNode.Count - 1;
				}

				ObservableCollection<ProjectObject> Description1Node = TypeNrNode[j].SubNode;
				j = find_index_in_collection(Description1Node, Description1Name[i]);
				if (j < 0)//hui
				{
					Description1Node.Add(new ProjectObject() { Name = Description1Name[i], SubNode = new ObservableCollection<ProjectObject>() });
					j = Description1Node.Count - 1;
				}

				ObservableCollection<ProjectObject> Description2Node = Description1Node[j].SubNode;
				j = find_index_in_collection(Description2Node, Description2Name[i]);
				if (j < 0)
				{
					Description2Node.Add(new ProjectObject() { Name = Description2Name[i], SubNode = new ObservableCollection<ProjectObject>() });
					j = Description2Node.Count - 1;
				}

				ObservableCollection<ProjectObject> Description3Node = Description2Node[j].SubNode;
				j = find_index_in_collection(Description3Node, Description3Name[i]);
				if (j < 0)
				{
					Description3Node.Add(new ProjectObject() { Name = Description3Name[i], SubNode = new ObservableCollection<ProjectObject>() });
					j = Description3Node.Count - 1;
				}

				ObservableCollection<ProjectObject> PartNrNode = Description3Node[j].SubNode;
				j = find_index_in_collection(PartNrNode, PartNrName[i]);
				if (j < 0)
				{
					PartNrNode.Add(new ProjectObject() { Name = PartNrName[i] });
					j = PartNrNode.Count - 1;
				}
			}

			//переименовываем элементы из столбца и сортируем по алфавиту названия из столбца supplier
			foreach (var node in Project){
				if (node.Name == "1"){
					node.Name = "Электротехника";
				}else if (node.Name == "2"){
					node.Name = "Fluid - техника";
				}else if (node.Name == "3"){
					node.Name = "Механика";
				}

			 	node.SubNode = new ObservableCollection<ProjectObject>(node.SubNode.OrderBy(i => i.Name));
			}

			#region Старые циклы)
			// for (int i = 0; i < ProductTopGroupName.Count; i++)
			// {
			//     ProductTopGroupNode.Add(new ProjectObject() { Name = ProductTopGroupName[i].ToString(), SubNode = new ObservableCollection<BaseObject>() });
			//     SupplierName = seq.Where(s => s.Field<short>("producttopgroup") == ProductTopGroupName[i]).Select(s => s.Field<string>("supplier")).Distinct().ToList();

			//     for (int j = 0; j < SupplierName.Count; j++)
			//     {
			//         SupplierNode.Add(new ProjectObject() { Name = SupplierName[j], SubNode = new ObservableCollection<BaseObject>() });
			//         var jj = SupplierNode[SupplierNode.IndexOf(SupplierNode.Last(x => x.Name == SupplierName[j]))];
			//         ProductTopGroupNode[i].SubNode.Add(jj);
			//         // ProductTopGroupNode[i].Tasks.Add(SupplierNode[j]);
			//         ProductGroupName = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
			//                                           (s.Field<string>("supplier") == SupplierName[j])).Select(s => s.Field<short>("productgroup")).Distinct().ToList();

			//         for (int k = 0; k < ProductGroupName.Count; k++)
			//         {
			//             ProductGroupNode.Add(new ProjectObject() { Name = ProductGroupName[k].ToString(), SubNode = new ObservableCollection<BaseObject>() });
			//             var kk = ProductGroupNode[ProductGroupNode.IndexOf(ProductGroupNode.Last(x => x.OriginName == ProductGroupName[k].ToString()))];
			//             jj.SubNode.Add(kk);
			//             //SupplierNode[j].Tasks.Add(ProductGroupNode[ProductGroupNode.IndexOf(ProductGroupNode.First(x => x.Name == ProductGroupName[k].ToString()))]);

			//             TypeNrName = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
			//                                         (s.Field<string>("supplier") == SupplierName[j]) &&
			//                                         (s.Field<short>("productgroup") == ProductGroupName[k])).Select(s => s.Field<string>("typenr")).Distinct().ToList();

			//             for (int l = 0; l < TypeNrName.Count; l++)
			//             {
			//                 TypeNrNode.Add(new ProjectObject() { Name = TypeNrName[l], SubNode = new ObservableCollection<BaseObject>() });
			//                 var ll = TypeNrNode[TypeNrNode.IndexOf(TypeNrNode.Last(x => x.Name == TypeNrName[l]))];
			//                 //ProductGroupNode[kk].Tasks.Add(TypeNrNode[TypeNrNode.IndexOf(TypeNrNode.First(x => x.Name == TypeNrName[l]))]);
			//                 kk.SubNode.Add(ll);

			//                 Description1Name = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
			//                                                   (s.Field<string>("supplier") == SupplierName[j]) &&
			//                                                   (s.Field<short>("productgroup") == ProductGroupName[k]) &&
			//                                                   (s.Field<string>("typenr") == TypeNrName[l])).Select(s => s.Field<string>("description1")).Distinct().ToList();

			//                 for (int m = 0; m < Description1Name.Count; m++)
			//                 {
			//                     Description1Node.Add(new ProjectObject() { Name = Description1Name[m], SubNode = new ObservableCollection<BaseObject>() });
			//                     var mm = Description1Node[Description1Node.IndexOf(Description1Node.Last(x => x.Name == Description1Name[m]))];
			//                     //TypeNrNode[l].Tasks.Add(Description3Node[Description3Node.IndexOf(Description3Node.First(x => x.Name == Description3Name[m]))]);
			//                     ll.SubNode.Add(mm);

			//                     Description2Name = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
			//                                                       (s.Field<string>("supplier") == SupplierName[j]) &&
			//                                                       (s.Field<short>("productgroup") == ProductGroupName[k]) &&
			//                                                       (s.Field<string>("typenr") == TypeNrName[l]) &&
			//                                                       (s.Field<string>("description1") == Description1Name[m])).Select(s => s.Field<string>("description2")).Distinct().ToList();

			//                     for (int n = 0; n < Description2Name.Count; n++)
			//                     {
			//                         Description2Node.Add(new ProjectObject() { Name = Description2Name[n], SubNode = new ObservableCollection<BaseObject>() });
			//                         var nn = Description2Node[Description2Node.IndexOf(Description2Node.Last(x => x.Name == Description2Name[n]))];
			//                         mm.SubNode.Add(nn);

			//                         //Description3Node[m].Tasks.Add(Description2Node[Description2Node.IndexOf(Description2Node.First(x => x.Name == Description2Name[n]))]);
			//                         Description3Name = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
			//                                                           (s.Field<string>("supplier") == SupplierName[j]) &&
			//                                                           (s.Field<short>("productgroup") == ProductGroupName[k]) &&
			//                                                           (s.Field<string>("typenr") == TypeNrName[l]) &&
			//                                                           (s.Field<string>("description1") == Description1Name[m]) &&
			//                                                           (s.Field<string>("description2") == Description2Name[n])).Select(s => s.Field<string>("description3")).Distinct().ToList();

			//                         for (int o = 0; o < Description3Name.Count; o++)
			//                         {
			//                             Description3Node.Add(new ProjectObject() { Name = Description3Name[o], SubNode = new ObservableCollection<BaseObject>() });
			//                             var oo = Description3Node[Description3Node.IndexOf(Description3Node.Last(x => x.Name == Description3Name[o]))];
			//                             nn.SubNode.Add(oo);

			//                             //Description2Node[n].Tasks.Add(Description1Node[Description1Node.IndexOf(Description1Node.First(x => x.Name == Description1Name[o]))]);
			//                             PartNrName = seq.Where(s => (s.Field<short>("producttopgroup") == ProductTopGroupName[i]) &&
			//                                                               (s.Field<string>("supplier") == SupplierName[j]) &&
			//                                                               (s.Field<short>("productgroup") == ProductGroupName[k]) &&
			//                                                               (s.Field<string>("typenr") == TypeNrName[l]) &&
			//                                                               (s.Field<string>("description1") == Description1Name[m]) &&
			//                                                               (s.Field<string>("description2") == Description2Name[n]) &&
			//                                                               (s.Field<string>("description3") == Description3Name[o])).Select(s => s.Field<string>("partnr")).Distinct().ToList();

			//                             for (int p = 0; p < PartNrName.Count; p++)
			//                             {
			//                                 PartNrNode.Add(new ProjectObject() { Name = PartNrName[p] });
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










		/*
        void InitStantoneProjectData(ProjectObject stantoneProject) {
            stantoneProject.Executor = "Ruben Ackerman";
            ProjectObject project21 = new ProjectObject() { Name = "Information Gathering", Executor = "Huyen Trinklein", Tasks = new ObservableCollection<BaseObject>() };
            project21.Tasks.Add(new Task() { Name = "Market research", Executor = "Tanner Crittendon", State = "Completed" });
            project21.Tasks.Add(new Task() { Name = "Making specification", Executor = "Carmine Then", State = "Completed" });

            ProjectObject project22 = new ProjectObject() { Name = "Planning", Executor = "Alfredo Sookoo", Tasks = new ObservableCollection<BaseObject>() };
            project22.Tasks.Add(new Task() { Name = "Documentation", Executor = "Gorf Wobbe", State = "Completed" });

            ProjectObject project23 = new ProjectObject() { Name = "Design", Executor = "Saphire Plump", Tasks = new ObservableCollection<BaseObject>() };
            project23.Tasks.Add(new Task() { Name = "Design of a web pages", Executor = "Dominic Minden", State = "In progress" });
            project23.Tasks.Add(new Task() { Name = "Pages layout", Executor = "Pinkerton Trezise", State = "In progress" });

            ProjectObject project24 = new ProjectObject() { Name = "Development", Executor = "Lauren Partain", Tasks = new ObservableCollection<BaseObject>() };
            project24.Tasks.Add(new Task() { Name = "Design", Executor = "Delilah Beamer", State = "In progress" });
            project24.Tasks.Add(new Task() { Name = "Coding", Executor = "Dunaway Dupriest", State = "Not started" });

            stantoneProject.Tasks.Add(project21);
            stantoneProject.Tasks.Add(project22);
            stantoneProject.Tasks.Add(new Task() { Name = "Task123", Executor = "Ex", State = "Completed" });
            stantoneProject.Tasks.Add(project23);
            stantoneProject.Tasks.Add(project24);
        }
        */






		#region Подключение к БД

		private void LoadFromDB()
		{

			#region из файла App.config используя класс ConfiguratorManager получим строку подключения
			// из файла App.config используя класс ConfiguratorManager получим строку подключения
			// string connectionString = ConfigurationManager.ConnectionStrings["Eplan"].ConnectionString;
			#endregion


			#region либо создадим эту строку сами напрямую
			// либо создадим эту строку сами напрямую
			string connectionString = @"Provider=Microsoft.JET.OLEDB.4.0; Data Source=C:\Users\bugrov\source\repos\WpfApp1\ESS_part002.mdb";
			#endregion

			#region либо воспользуемся классом OleDbConnectionStringBuilder
			/* // либо воспользуемся классом OleDbConnectionStringBuilder
            OleDbConnectionStringBuilder OleDbCSB = new OleDbConnectionStringBuilder();
            OleDbCSB.Provider = "Microsoft.JET.OLEDB.4.0";
            OleDbCSB.DataSource = @"C:\Users\bugrov\source\repos\WpfApp1\ESS_part002.mdb";
            string ConnectionString = OleDbCSB.ConnectionString; */
			#endregion

			//Создадим объект подключения и в конструкор передадим адрес базы
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				string SQLQuery = "SELECT * FROM tblPart";

				try
				{
					connection.Open();
					if (connection.State == ConnectionState.Open)
					{
						MessageBox.Show("Подключились к базе. Путь и имя базы " + connection.DataSource);
						// MessageBox.Show(connection.Provider);


						//создадим объект-команду, которая отсылает запрос на сревер (в БД) и в конструкто сразу передадим соединение и строку запроса
						using (OleDbCommand command = new OleDbCommand(SQLQuery, connection))
						{
							//Отправляем команду на сервер и запоминаем результат в объект OleDbDataReader
							OleDbDataReader reader = command.ExecuteReader();

							//  mylist = new ObservableCollection<string>();

							while (reader.Read())
							{

								// ListView1.Items.Add(new ListViewItem {Content=reader["description1"].ToString()});

							}
						}



						#region либо создадим объект-команду, которая отсылает запрос на сревер (в БД) и сами передадим этому объекту соединение
						/*  //либо создадим объект-команду, которая отсылает запрос на сревер (в БД) и сами передадим этому объекту соединение
                          OleDbCommand command = new OleDbCommand();
                          command.Connection = connection;
                          //Вносим SQL  запрос, который нужно отправить на сервер. Запрашиваем количество записей в таблице tblPart
                          command.CommandText = "SELECT COUNT(*) FROM tblPart";
                          //Отправляем команду на сервер и запоминаем результат в переменную.
                          //Метод ExecuteScalar подходит для запросов, которые возвращают только одно значение
                          int count = (int)command.ExecuteScalar();
                          MessageBox.Show(count.ToString()); */
						#endregion

						#region либо создадим объект-команду, которая отсылает запрос на сревер (в БД) при помощи метода CreateCommand
						/*   //либо создадим объект-команду, которая отсылает запрос на сревер (в БД) при помощи метода CreateCommand
                             OleDbCommand command = connection.CreateCommand();
                             command.CommandText="SELECT COUNT(*) FROM tblPart";
                             //Отправляем команду на сервер и запоминаем результат в переменную.
                             //Метод ExecuteScalar подходит для запросов, которые возвращают только одно значение
                             int count = (int)command.ExecuteScalar();
                             MessageBox.Show(count.ToString()); */
						#endregion


						connection.Close();
					}
				}
				catch (Exception ex)
				{

					MessageBox.Show(ex.Message);
				}
			}


		}

		#endregion
	}


}

