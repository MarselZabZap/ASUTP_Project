   M S B A M L   `   `   `  ����O  KEquipmentsAccounting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:EquipmentsAccounting.viewsEquipmentsAccounting.views  5      *    $EquipmentsAccounting.views.StockPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:EquipmentsAccounting.views     5      -   5          N� Title$  	StockPage��5
      .��5      � 
������� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      .+�5       � $��0��6    � Orientation$ Vertical=�5!      $��Center=�5"      .+�5$      �� $��Фильтрация��6   $��5,0,0,0q�5%      $��14(�5&      �� 5(      -   5         ��Name$ FilterTextBox��5(      $	��250��5)      $��
5, 0, 5, 5q�5*      $��14(�5+      5,      �� 5.      -   5         ��Name$ StocksComboBox��5.      $��14(�5/      $	��250��50      $��25��51      $
��Left=�52      $��5,0,5,5q�53      $��Center=�54      55      57      � 59      $��1��6   $ Vertical=�5:      $��Center=�5;      $��	15,0,30,0q�5<      .+�5>      	� -   5         	�Name$ TypeRadioButton��5>       d� 	IsChecked$
 TrueY�5?      $��По типу��5@      $��Center=�5A      	� 5C      -   5        $ CharsRadioButton��5C      $)��#По характеристикам��5D      $��Center=�5E      	� 5G      -   5        $ SerialNumRadioButton��5G      $*��$По серийному номеру��5H      $��Center=�5I      5K      � 5M      $ 
Horizontal=�6   $��2��5N      $��Center=�5O      .+�5Q      �� -   5         ��Name$ OpenTransferWindowButton��5Q      $��Предать��5R      $��14(�5S      $��	10,2,10,4q�5T      $��25��5U      5V      �� 5X      -   5        
���   �$��1q�5Y      ���#dbdbdb$��36��5[      $��36��5\      $��20,0,0,0q�5]      	 � Cursor$
	 Hand��5_      ._�5a      �� $A��;E:\!АСУТП Проект!\ASUTP_Project\img\doc_icon.png��6   $��30��5b      5c      5d      5f      &   System.Windows.Controls.DataGrid  5h   
   -	   5        
  Name$
 StockInfoDataGrid��5h      $��1��5j         
IsReadOnly .    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5l      ��5i      ��    	CellStyle 5m      �� 5n       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   5o      5q      5s      5t      