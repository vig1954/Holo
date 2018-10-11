using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Processing;
using Processing.DataAttributes;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace ManualTesting
{
    public partial class frmValueBindingTesting : Form
    {
        public frmValueBindingTesting()
        {
            InitializeComponent();
        }

        private void frmValueBindingTesting_Load(object sender, EventArgs e)
        {
            Test();
        }

        private void Test()
        {
            var testSubject = new TestSubject3();

            var interfaceController = new InterfaceController(testHostPanel);
            interfaceController.BindObjectToInterface(testSubject);
        }

        private class TestSubject
        {
            public IBindingManager<TestSubject> MyBindingManager { get; set; }

            [BindToUI("Custom name", "Display Group 1"), Precision(0)]
            public int IntegerValue { get; set; } = 1;

            [BindToUI(displayGroup: "Button?")]
            public void Action1() => Debug.WriteLine("Action1 Clicked!");

            [BindToUI(hideLabel: true), Precision(2)]
            public float FloatValue { get; set; } = -2.3f;

            [BindToUI("ACTION 2", "Display Group 1")]
            public void Action2() => MyBindingManager.SetAvailableValuesForProperty(o => o.ValueWithAvailableValueRange, new[] { "One", "Two", "Three" });

            [BindToUI, Precision(10)]
            public double DoubleValue { get; set; } = 4.5678901234;

            [BindToUI]
            public bool Flag { get; set; } = true;

            [BindToUI]
            public bool Flag2 { get; set; } = false;

            [BindToUI]
            public TestEnum TestEnumValue { get; set; } = TestEnum.EnumItem3;

            [BindToUI]
            [ValueCollection("Value", "Other Value", "pom pom")]
            public string ValueWithAvailableValueRange { get; set; } = "Value";

            [BindToUI]
            public IImageHandler ImageHandler { get; set; }

            [BindToUI]
            public Point Point { get; set; } = new Point(1, 2);

            [BindToUI]
            public Rectangle Rectangle { get; set; } = new Rectangle(0, 0, 100, 100);

            [BindToUI]
            [BindMembersToUI]
            public TestSubject2 MyTestSubject { get; set; }

            [BindToUI]
            public void InitializeMyTestSubject() => MyBindingManager.SetPropertyValue(o => o.MyTestSubject, new TestSubject2 { Title = "Hello", Count = 2 });
            
            [BindToUI]
            public void NullifyMyTestSubject() => MyBindingManager.SetPropertyValue(o => o.MyTestSubject, null);

            public enum TestEnum
            {
                EnumItem1,
                EnumItem2,
                EnumItem3,
                EnumItem4
            }
        }

        private class TestSubject2
        {
            [BindToUI]
            public string Title { get; set; }

            [BindToUI]
            public int Count { get; set; }
        }

        private class TestSubject3
        {
            [BindToUI, BindMembersToUI]
            public TestSubject2 Member { get; set; } = new TestSubject2 { Title = "Title", Count = 2 };
        }
    }
}