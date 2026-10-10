
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bai54_QuanLyNhanVien
{
    public partial class Form1 : Form
    {
        private SplitContainer split = new SplitContainer();
        private TreeView tvDepartments = new TreeView();
        private ListView lsvEmployees = new ListView();
        private ComboBox cboView = new ComboBox();
        private ImageList images = new ImageList();

        private class Employee
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Position { get; set; }
            public DateTime StartDate { get; set; }
            public string Department { get; set; }
            public string Group { get; set; }

            public Employee(string id, string name, string position,
                DateTime startDate, string department, string group)
            {
                Id = id;
                Name = name;
                Position = position;
                StartDate = startDate;
                Department = department;
                Group = group;
            }
        }

        private List<Employee> employees = new List<Employee>();

        public Form1()
        {
            InitializeComponent();

            Text = "Bài 5.4 - Quản lý nhân viên";
            Size = new Size(1100, 650);
            StartPosition = FormStartPosition.CenterScreen;

            CreateData();
            CreateInterface();
            CreateTree();
            ShowEmployees(null!);
        }

        private void CreateData()
        {
            employees.Add(new Employee("NV001", "Nguyễn Văn An",
                "Trưởng phòng", new DateTime(2020, 3, 15),
                "Phòng Kỹ thuật", "Nhóm Phát triển"));

            employees.Add(new Employee("NV002", "Trần Thị Bình",
                "Lập trình viên", new DateTime(2022, 6, 10),
                "Phòng Kỹ thuật", "Nhóm Phát triển"));

            employees.Add(new Employee("NV003", "Lê Minh Cường",
                "Kiểm thử viên", new DateTime(2023, 1, 20),
                "Phòng Kỹ thuật", "Nhóm Kiểm thử"));

            employees.Add(new Employee("NV004", "Phạm Thu Hà",
                "Kế toán", new DateTime(2021, 9, 1),
                "Phòng Tài chính", "Nhóm Kế toán"));

            employees.Add(new Employee("NV005", "Hoàng Quốc Dũng",
                "Nhân viên nhân sự", new DateTime(2024, 2, 12),
                "Phòng Nhân sự", "Nhóm Tuyển dụng"));
        }

        private void CreateInterface()
        {
            Label title = new Label
            {
                Text = "TRÌNH QUẢN LÝ NHÂN VIÊN",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            Controls.Add(title);

            Label lblView = new Label
            {
                Text = "Chế độ xem:",
                AutoSize = true,
                Location = new Point(720, 25)
            };
            Controls.Add(lblView);

            cboView.SetBounds(810, 20, 220, 30);
            cboView.DropDownStyle = ComboBoxStyle.DropDownList;
            cboView.Items.AddRange(new object[]
            {
                "Details", "SmallIcon", "LargeIcon", "Tile", "List"
            });
            cboView.SelectedIndexChanged += (s, e) =>
            {
                switch (cboView.Text)
                {
                    case "Details":
                        lsvEmployees.View = View.Details;
                        break;
                    case "SmallIcon":
                        lsvEmployees.View = View.SmallIcon;
                        break;
                    case "LargeIcon":
                        lsvEmployees.View = View.LargeIcon;
                        break;
                    case "Tile":
                        lsvEmployees.View = View.Tile;
                        break;
                    case "List":
                        lsvEmployees.View = View.List;
                        break;
                }
            };
            Controls.Add(cboView);

            split.SetBounds(20, 65, 1040, 520);
            split.SplitterDistance = 300;
            split.Dock = DockStyle.None;
            Controls.Add(split);

            tvDepartments.Dock = DockStyle.Fill;
            tvDepartments.HideSelection = false;
            tvDepartments.ImageList = images;
            tvDepartments.AfterSelect += (s, e) =>
            {
                ShowEmployees(e.Node);
            };
            split.Panel1.Controls.Add(tvDepartments);

            lsvEmployees.Dock = DockStyle.Fill;
            lsvEmployees.View = View.Details;
            lsvEmployees.FullRowSelect = true;
            lsvEmployees.GridLines = true;
            lsvEmployees.SmallImageList = images;
            lsvEmployees.LargeImageList = images;

            lsvEmployees.Columns.Add("Mã NV", 100);
            lsvEmployees.Columns.Add("Họ Tên", 200);
            lsvEmployees.Columns.Add("Chức vụ", 160);
            lsvEmployees.Columns.Add("Ngày vào làm", 130);

            split.Panel2.Controls.Add(lsvEmployees);

            images.ImageSize = new Size(32, 32);
            images.ColorDepth = ColorDepth.Depth32Bit;
            images.Images.Add("company", SystemIcons.Application.ToBitmap());
            images.Images.Add("department", SystemIcons.WinLogo.ToBitmap());
            images.Images.Add("group", SystemIcons.Information.ToBitmap());
            images.Images.Add("employee", SystemIcons.Question.ToBitmap());

            cboView.SelectedIndex = 0;
        }

        private void CreateTree()
        {
            TreeNode company = new TreeNode("Công ty ABC");
            company.ImageKey = "company";
            company.SelectedImageKey = "company";

            TreeNode technical = CreateDepartment("Phòng Kỹ thuật");
            technical.Nodes.Add(CreateGroup("Nhóm Phát triển"));
            technical.Nodes.Add(CreateGroup("Nhóm Kiểm thử"));

            TreeNode finance = CreateDepartment("Phòng Tài chính");
            finance.Nodes.Add(CreateGroup("Nhóm Kế toán"));

            TreeNode hr = CreateDepartment("Phòng Nhân sự");
            hr.Nodes.Add(CreateGroup("Nhóm Tuyển dụng"));

            company.Nodes.Add(technical);
            company.Nodes.Add(finance);
            company.Nodes.Add(hr);

            tvDepartments.Nodes.Add(company);
            company.Expand();
            technical.Expand();

            tvDepartments.SelectedNode = company;
        }

        private TreeNode CreateDepartment(string name)
        {
            return new TreeNode(name)
            {
                ImageKey = "department",
                SelectedImageKey = "department"
            };
        }

        private TreeNode CreateGroup(string name)
        {
            return new TreeNode(name)
            {
                ImageKey = "group",
                SelectedImageKey = "group"
            };
        }

        private void ShowEmployees(TreeNode? node)
        {
            lsvEmployees.Items.Clear();

            if (node == null)
                return;

            string selected = node.Text;
            IEnumerable<Employee> result;

            if (selected == "Công ty ABC")
            {
                result = employees;
            }
            else if (selected.StartsWith("Phòng "))
            {
                result = employees.Where(e => e.Department == selected);
            }
            else
            {
                result = employees.Where(e => e.Group == selected);
            }

            foreach (Employee emp in result)
            {
                ListViewItem item = new ListViewItem(emp.Id);
                item.SubItems.Add(emp.Name);
                item.SubItems.Add(emp.Position);
                item.SubItems.Add(emp.StartDate.ToString("dd/MM/yyyy"));
                item.ImageKey = "employee";
                lsvEmployees.Items.Add(item);
            }
        }
    }
}
