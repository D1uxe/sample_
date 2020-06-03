using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChildNodesPathDemo {

    public class BaseObject {
        public string Name { get; set; }
        public string Executor { get; set; }
       // public string Image { get; set; }
       // public BitmapImage  Image = new BitmapImage(new Uri("Resources/_Folder.ico", UriKind.Relative)); 
        public override string ToString() { return Name; }

        protected static BitmapImage StaticImage;
        public BaseObject()
        {
            StaticImage = new BitmapImage(new Uri("/Resources/_Folder.ico", UriKind.Relative));
        }
        public BitmapImage Image { get { return StaticImage; } }
    }

    public class ProjectObject : BaseObject {
        public ObservableCollection<BaseObject> Tasks { get; set; }
        
    }

    public class Task : BaseObject {
        public string State { get; set; }
    }

    public class ViewModel {
        public ObservableCollection<ProjectObject> DataItems { get; set; }

        public ViewModel() {
            DataItems = InitData();
        }

        private ObservableCollection<ProjectObject> InitData() 
        {
            ObservableCollection<ProjectObject> projects = new ObservableCollection<ProjectObject>();
            ProjectObject betaronProject = new ProjectObject() { Name = "ProductTopGroup Электротехника",  Tasks = new ObservableCollection<BaseObject>() };
            ProjectObject stantoneProject = new ProjectObject() { Name = "ProductTopGroup Механика", Tasks = new ObservableCollection<BaseObject>() };
            
            

            InitBetaronProjectData(betaronProject);
            //InitStantoneProjectData(stantoneProject);

            projects.Add(betaronProject);
            projects.Add(stantoneProject);

            return projects;
        }

        void InitBetaronProjectData(ProjectObject betaronProject) 
        {

            // либо создадим эту строку сами напрямую
            string connectionString = @"Provider=Microsoft.JET.OLEDB.4.0; Data Source=C:\Users\bugrov\source\repos\WpfApp1\ESS_part002.mdb";

            //Создадим объект класса DataSet, в который происходит копирование из OleDbDataAdapter
             DataSet dataSet = new DataSet();


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



            betaronProject.Executor = "Mcfadyen Ball";
            
            
            DataRow row = dataSet.Tables[0].Rows[0];
            
            string str = dataSet.Tables[0].Rows[0].ItemArray[71].ToString(); // только для текущей тестовой базы 55-partnr 21-description1 22-description2 26-erpnr 33-groupnumber 38-id 45-maintanance 46-manifacturer 50-note 51-ordernr 59-productgroup 71-supplier 73-typenr

            ProjectObject suppl = new ProjectObject() { Name ="Supplier " + str, Executor = dataSet.Tables[0].Rows[0].ItemArray[55].ToString(), Tasks = new ObservableCollection<BaseObject>() };

                       
           // project11.Tasks.Add(new Task() { Name = "Market research", Executor = "Carmine Then", State = "Completed" });
            //project11.Tasks.Add(new Task() { Name = "Making specification", Executor = "Seto Kober", State = "In progress" });

            ProjectObject prodgroup = new ProjectObject() { Name = "ProductGroup Клеммы " + dataSet.Tables[0].Rows[0].ItemArray[59].ToString(), Executor = "Manley Difrancesco", Tasks = new ObservableCollection<BaseObject>() };
           // project12.Tasks.Add(new Task() { Name = "Tddfgr ", Executor = "Martez Gollin", State = "Not started" });

            ProjectObject typenr = new ProjectObject() { Name = "TypeNr " + dataSet.Tables[0].Rows[0].ItemArray[73].ToString(), Tasks = new ObservableCollection<BaseObject>() };
          //  project112.Tasks.Add(new Task() { Name = "afafsasfafs", Executor = "Maasfafs", State = "Not asfafs" });

           // project12.Tasks.Add(project112);
           // project11.Tasks.Add(project12);
            
            ProjectObject desc1 = new ProjectObject() { Name = "Descr1 " + dataSet.Tables[0].Rows[0].ItemArray[21].ToString(), Tasks = new ObservableCollection<BaseObject>() };
            ProjectObject desc2 = new ProjectObject() { Name = "Descr2 " + dataSet.Tables[0].Rows[0].ItemArray[22].ToString(), Tasks = new ObservableCollection<BaseObject>() };
            desc2.Tasks.Add(new Task {Name="Заказн "+ dataSet.Tables[0].Rows[0].ItemArray[55].ToString(), Executor="Описалово  "+ dataSet.Tables[0].Rows[0].ItemArray[50].ToString() });

            // ProjectObject prpartnr = new ProjectObject() { Name = "Descr1 " + dataSet.Tables[0].Rows[0].ItemArray[55].ToString() };

            suppl.Tasks.Add(prodgroup);
            prodgroup.Tasks.Add(typenr);
            typenr.Tasks.Add(desc1);
            desc1.Tasks.Add(desc2);

            // project13.Tasks.Add(new Task() { Name = "Design of a web pages", Executor = "Gasper Hartsell", State = "Not started" });
            // project13.Tasks.Add(new Task() { Name = "Pages layout", Executor = "Shirish Huminski", State = "Not started" });

            // project11.Tasks.Add(project13);

            betaronProject.Tasks.Add(suppl);
           // betaronProject.Tasks.Add(project12);
           // betaronProject.Tasks.Add(project13);
            betaronProject.Tasks.Add(new Task() { Name = "Task007", Executor = "Ex", State = "Completed" });

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
            
        private void LoadFromDB ()
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

