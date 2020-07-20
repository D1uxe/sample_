﻿using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System.Windows;

namespace ChildNodesPathDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeListView_StartRecordDrag(object sender, DevExpress.Xpf.Core.StartRecordDragEventArgs e)
        {
            var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));

            if (((ProjectObject)data.Records[0]).Executor == null)
            {
                e.AllowDrag = false;
                e.Handled = true;
            }


            //foreach (ProjectObject po in data.Records)
            //{
            //    this.Title = po.Executor;
            //}


            if (e.Data.GetDataPresent(typeof(ViewModel)))
            {

            }



            // if (e.Data.GetDataPresent(typeof(ViewModel.PartNrNode)))
            //if (e.Records.Any(x=>x.)
            //{
            //    this.Title = "Yes";

            //}
            //else
            //this.Title = "No";

        }

        private void TreeListView_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            

        }

        private void TreeListView_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //if (((TreeListView)sender).FocusedRowData.IsSelected)
            //{
            //    this.Title = "Selected" + " " + ((TreeListView)sender).FocusedRowData.Level.ToString() +"leave";
            //}
            //if (((TreeListView)sender).FocusedRowData.IsFocused)
            //{
            //    this.Title = "Focused" + " " + ((TreeListView)sender).FocusedRowData.Level.ToString() + "leave";
            //}

       

        }

        private void TreeListView_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {



            //    if (((TreeListView)sender).FocusedRowData.IsSelected)
            //{
            //    this.Title = "Selected" + " " + ((TreeListView)sender).FocusedRowData.Level.ToString();
            //}
            //if (((TreeListView)sender).FocusedRowData.IsFocused)
            //{
            //    this.Title = "Focused" + " " + ((TreeListView)sender).FocusedRowData.Level.ToString();
            //}


            //свойство Content выделенного узла содержит ProjectObject со всеми свойствами.
            // this.Title = ((ProjectObject)((TreeListView)sender).FocusedNode.Content).Executor ?? "Null";
            this.Title = ((TreeListView)sender).FocusedNode.Id.ToString();
            
        }

        private void TreeListView_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((ViewModel)this.DataContext).Foo();
        }
    }

}
