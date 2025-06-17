using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoMind;
using PhysLab.DB;

namespace PhysLab;

public static class HelperExtentions
{
    public static List<TreeViewItem> ToTreeViewItem(this CalculatingEnvironment environment)
    {
        var items = new List<TreeViewItem>();
        foreach (var pack in environment.ImportPacks)
        {
            var item = new TreeViewItem { Header = pack.ToString() };
            var pi = new TreeViewItem { Header = "Свойства" };
            var fi = new TreeViewItem { Header = "Функции" };
            var ci = new TreeViewItem { Header = "Константы" };

            foreach (var property in pack.Properties)
            {
                var treeViewItem = new TreeViewItem { DataContext = property, Header = property };
                treeViewItem.MouseDown += TreeViewItemOnMouseDown;
                pi.Items.Add(treeViewItem);
            }

            foreach (var formula in pack.Formulas)
            {
                var treeViewItem = new TreeViewItem { DataContext = formula, Header = formula };
                treeViewItem.MouseDown += TreeViewItemOnMouseDown;
                fi.Items.Add(treeViewItem);
            }

            foreach (var constant in pack.Constants)
            {
                var treeViewItem = new TreeViewItem { DataContext = constant, Header = $"{constant} : {Math.Round(constant.Value, 4)}" };
                treeViewItem.MouseDown += TreeViewItemOnMouseDown;
                ci.Items.Add(treeViewItem);
            }

            item.Items.Add(pi);
            item.Items.Add(fi);
            item.Items.Add(ci);
            items.Add(item);
        }

        return items;
    }

    private static void TreeViewItemOnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var treeViewItem = sender as TreeViewItem;
        DragDrop.DoDragDrop(treeViewItem, treeViewItem.DataContext, DragDropEffects.Link);
    }
}