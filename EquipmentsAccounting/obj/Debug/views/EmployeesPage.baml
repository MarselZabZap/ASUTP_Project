   M S B A M L   `   `   `  ����O  KEquipmentsAccounting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:EquipmentsAccounting.viewsEquipmentsAccounting.views  5      .    (EquipmentsAccounting.views.EmployeesPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:EquipmentsAccounting.views     5      -   5          N� Title$  EmployeesPage��5
      .��5      � 
������� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   � $��0��6    � Orientation$ 
Horizontal=�5      .+�5      � $ Vertical=�6   .+�5      �� $��Фильтрация��6   $��Center=�5      $��14(�5      $��10,5,5,0q�5      �� 5       -   5         ��Name$ FilterTextBox��5       $��14(�5!      $��10q�5"      $
��Left=�5#      $	��200��5$      $��20��5%      5&      5(      �� 5*      -   5         ��Name$ 
GiveButton��5*      $��Выдать��5+      $��Center=�5-      $��	30,10,0,0q�5.      $��70��5/      $��5q�50      �� 52      -   5        $ 	GetButton��52      $��
Снять��53      $��Center=�55      $��	30,10,0,0q�56      $��70��57      $��5q�58      �� 5:      -   5        $ HandOverButton��5:      $��Передать��5;      $��Center=�5=      $��	30,10,0,0q�5>      $��70��5?      $��5q�5@      5B      &   System.Windows.Controls.DataGrid  5F   
   -   5          Name$ EmployeesInfoDataGrid��5F      $��1��5H         
IsReadOnly .    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5J      ��5G      ��    	CellStyle 5L      �� 5N      	 �� 
TargetType*  $System.Windows.Controls.DataGridCell"	  6   .�5O      -   5        5P      5T      �� 5V   
   $��2��6   .@�5X      �� $��Оборудование��6   .��5Z        -   5        $ EquipmentsInfoDataGrid��5Z       . $ * 5]      ��5[      ��  5_      �� 5a      "	  6    .�5b      -	   5        5c      5g      5i      5k      5m      5n      