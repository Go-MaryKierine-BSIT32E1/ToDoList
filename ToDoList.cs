using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.ComponentModel;

namespace ToDoListApp
{
    [Serializable]
    public class ToDoItem : INotifyPropertyChanged
    {
        private XElement todo;

        internal ToDoItem(XElement todo)
        {
            this.todo = todo;
        }

        public string CreatedDateTime
        {
            get { return todo.Element("createdatetime").Value; }
            set
            {
                todo.Element("createdatetime").Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreatedDateTime)));
            }
        }

        public string Description
        {
            get { return todo.Element("description").Value; }
            set
            {
                todo.Element("description").Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public string DoneDateTime
        {
            get { return todo.Element("donedatetime")?.Value ?? ""; }
            set
            {
                if (todo.Element("donedatetime") == null)
                {
                    todo.Add(new XElement("donedatetime"));
                }

                todo.Element("donedatetime").Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoneDateTime)));
            }
        }

        public bool IsDone { get { return todo.Element("donedatetime") != null; } }

        public override string ToString()
        {
            string result = DoneDateTime != "" ? $"{Description} - Done at: {DateTime.Parse(DoneDateTime):yyyy-MM-dd hh:mm}" : Description;
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [Serializable]
    public class ToDoList : INotifyPropertyChanged
    {
        private XDocument todoList;
        private bool showDoneItems;
        private List<ToDoItem> items;

        public List<ToDoItem> Items
        {
            get
            {
                return showDoneItems ? items : items.Where(i => i.DoneDateTime == "").ToList();
            }
        }

        public bool ShowDoneItems
        {
            get { return showDoneItems; }
            set
            {
                showDoneItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowDoneItems)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
            }
        }

        public ToDoList()
        {
            string listFileName = ConfigurationManager.AppSettings["listFileName"];
            if (File.Exists(listFileName))
            {
                todoList = XDocument.Load(listFileName);
                items = todoList.Root.Elements().Select(e => new ToDoItem(e)).ToList();
            }
            else
            {
                todoList = new XDocument(new XElement("todolist"));
                items = new List<ToDoItem>();
            }
        }

        public void AddItem(string description)
        {
            XElement item = new XElement("todo",
                new XElement("createdatetime", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ms")),
                new XElement("description", description));

            todoList.Root.Add(item);
            items.Add(new ToDoItem(item));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
        }

        public void DeleteItem(ToDoItem item)
        {
            items.Remove(item);
            item.ToDo.Remove();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
            string listFileName = ConfigurationManager.AppSettings["listFileName"];
            todoList.Save(listFileName);
        }
    }
}
