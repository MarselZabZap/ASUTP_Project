   M S B A M L   `   `   `  ����O  KEquipmentsAccounting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:EquipmentsAccounting.windowsEquipmentsAccounting.windows  5   	   7    1EquipmentsAccounting.windows.IssueEquipmentWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:EquipmentsAccounting.windows     5   	     � Title$+  %Выдача оборудования��5   	   $	��680��6%   $	��720��5	   	   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $	��200 �6   .+�5   
   �� $��
Склад��6   $��0��5      $��	6,6,-6,35q�5      $��2��5      $��20(�5      	� 5   
   -   5         	�Name$ TypeRadioButton��5       d� 	IsChecked$
 TrueY�5      $��По типу��5      $
��Left=�5      $��
280,15,0,0q�5      $��0��5      $	��Top=�5       J� RenderTransformOrigin$ 0.236,0.735&�5      	� 5!   
   -   5        $ CharsRadioButton��5!      $)��#По характеристикам��5"      $
��Left=�5#      $��0��65   $	��Top=�5$      $��
364,15,0,0q�62   	� 5&   
   -   5        $ SerialNumRadioButton��5&      $*��$По серийному номеру��5'      $
��Left=�5(      $��
517,15,0,0q�5)      $	��Top=�5*      �� 5,   
   -   5         ��Name$ FilterTextBox��5,      $
��Left=�5-      $��	78,14,0,0q�5.       �� TextWrapping$
 Wrap=�5/      $	��Top=�50      $	��187��51      $��12(�52      53      &   System.Windows.Controls.DataGrid  55   
   -   5          Name$ EquipmentsDataGrid��55      $��0��57         
IsReadOnly . 	   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$	 * 59      $��0,45,0,0q�6'   ��56      �� 
   	CellStyle
 5:      �� 5;       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   .�5<      -   5        5=      5?        5A   
   -   5        $  SelectedEquipmentsDataGrid��5A       . $	 * 5D      $��
0,464,0,39q�6'   $��2��6;   ��5B      �� 
 5E      �� 5F      "  6   .�5G      -   5        5H      5J      �� 5L   
   -	   5         ��Name$ IssueButton��5L      $��Выдать��5M      $��Center=�5N      $��	0,168,0,0q�5O      $��1��5P      $	��Top=�5Q      $��81��5R      $��14(�5S      5T      �� 5V   
   -
   5         ��Name$ Notification��5V       �� Orientation$ 
Horizontal=�5W      $��3��5X      
������$��Bottom=�5Z      $��25��5[      $y�	Collapsed=�5\      .+�5^      �� $��Успешно��6   $��10,0,0,0q�5_      5a      5c      5f      