using GAT_Produkcja.Startup;
using GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Dodaj;
using NUnit.Framework;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Mieszanka.Dodaj
{
    [Ignore("probelm z ComonServiceLocator")]
    [TestFixture]
    public class MieszankaDodajViewTests
    {
        [Test, Apartment(System.Threading.ApartmentState.STA)]
        public void CheckWpfBindingsAreValid()
        {
            // instansiate the xaml view and set DataContext
            ViewModelLocator locator = new ViewModelLocator();
            var yourView = new MieszankaDodajView();
            //yourView.DataContext = new MieszankaDodajViewModel();

            FindBindingsRecursively(yourView,
                    delegate (FrameworkElement element, Binding binding, DependencyProperty dp)
                    {
                        var type = yourView.DataContext.GetType();

                // check that each part of binding valid via reflection
                foreach (string prop in binding.Path.Path.Split('.'))
                        {
                            PropertyInfo info = type.GetProperty(prop);
                            Assert.IsNotNull(info);
                            type = info.PropertyType;
                        }
                    });
        }

        private delegate void FoundBindingCallbackDelegate(FrameworkElement element, Binding binding, DependencyProperty dp);

        private void FindBindingsRecursively(DependencyObject element, FoundBindingCallbackDelegate callbackDelegate)
        {
            // See if we should display the errors on this element
            MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Static |
                        BindingFlags.Public |
                        BindingFlags.FlattenHierarchy);

            foreach (MemberInfo member in members)
            {
                DependencyProperty dp = null;

                // Check to see if the field or property we were given is a dependency property
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(field.FieldType))
                    {
                        dp = (DependencyProperty)field.GetValue(element);
                    }
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo prop = (PropertyInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(prop.PropertyType))
                    {
                        dp = (DependencyProperty)prop.GetValue(element, null);
                    }
                }

                if (dp != null)
                {
                    // Awesome, we have a dependency property. does it have a binding? If yes, is it bound to the property we're interested in?
                    Binding bb = BindingOperations.GetBinding(element, dp);
                    if (bb != null)
                    {
                        // This element has a DependencyProperty that we know of that is bound to the property we're interested in. 
                        // Now we just tell the callback and the caller will handle it.
                        if (element is FrameworkElement)
                        {
                            callbackDelegate((FrameworkElement)element, bb, dp);
                        }
                    }
                }
            }

            // Now, recurse through any child elements
            if (element is FrameworkElement || element is FrameworkContentElement)
            {
                foreach (object childElement in LogicalTreeHelper.GetChildren(element))
                {
                    if (childElement is DependencyObject)
                    {
                        FindBindingsRecursively((DependencyObject)childElement, callbackDelegate);
                    }
                }
            }
        }
    }
}
