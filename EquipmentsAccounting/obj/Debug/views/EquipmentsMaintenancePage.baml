   M S B A M L   `   `   `  ����O  KEquipmentsAccounting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:EquipmentsAccounting.viewsEquipmentsAccounting.views  5      :    4EquipmentsAccounting.views.EquipmentsMaintenancePage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:EquipmentsAccounting.views     5      -   5          N� Title$  EquipmentsMaintenancePage��5       N� 
Background
 �����.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6    � Orientation$ 
Horizontal=�5      .+�5      � $ Vertical=�6   $��Center=�5      .+�5      �� $��Фильтрация��6   $��Center=�5      $��14(�5       $��10,0,5,0q�5!      �� 5#      -   5         ��Name$ FilterTextBox��5#      $��14(�5$      $��10q�5%      $
��Left=�5&      $	��200��5'      $��20��5(      5)      5+      � 5-      $ Vertical=�6   $��Center=�5.      .+�50      	� -   5         	�Name$ NameRadioButton��50       d� 	IsChecked$
 TrueY�51      $��14(�52      $#��По наименованию��53      $��Center=�54      $��	10,0,10,0q�55      	� 57      -   5        $ MaintenanceRadioButton��57      $#��По обслуживанию��58      $��14(�59      $��Center=�5:      $��	10,0,10,0q�5;      �� 5=      -   5         ��Name$ WithoutEndDateCheckBox��5=      $ ��Незавершённые��5>      $��14(�5?      $��Center=�5@      $��	10,0,10,0q�5A      5B      5D      �� 5F      -   5         ��Name$ 	DateLabel��5F      $6��0______._____.__________ - _____._____.__________��5G      $��14(�5H      $��12,0,0,0q�5I      $��Bottom=�5J      $��Center=�5K      $��0,0,0,0q�5L      	 � Cursor$
	 Hand��5N      d� 5P      -   5        
 d�Name$ 
 DateTypeFilterToggleButton��5P      $��
Приём��5Q      $��Center=�5R      $��10,0,0,0q�5S      $��13(�5T      $��5,2,5,3q�5U      5V      �� 5X      -   5         ��Name$ CalendarImage��5Y      $~��xE:\!АСУТП Проект!\PROJECT\EquipmentsAccounting(Desktop) Отлаженая версия\img\calendar_icon.png��5X      $��35��5Z      $��15,0,0,0q�5[      $
	 Hand��5]      5_      &   System.Windows.Controls.DataGrid  5b   
   -	   5          Name$# EquipmentsMaintenanceDataGrid��5b      $��1��5c         
IsReadOnly .    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5f      ��5d      ��    	CellStyle 5h      �� 5i       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   .�5j      -
   5        5k      5n      � 5q   
   $��2��6   $��Center=�5r      .+�5t      �� -   5         ��Name$ RemoveMaintenanceButton��5t      $,��&Снять с обслуживания��5u      $��Center=�5v      $��Center=�5w      $��
0,10,10,10q�5x      $��5q�5y      {�.  5{      �� 5}      -   5        $( "RemoveMaintenanceButtonWithChanges��5}      $F��@Снять с обслуживания с изменениями��5~      $��Center=�5      $��Center=�5�      $��
10,10,0,10q�5�      $��5q�5�      {�.  5�      5�      5�      5�      