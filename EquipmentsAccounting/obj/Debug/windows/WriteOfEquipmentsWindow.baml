   M S B A M L   `   `   `  ����O  KEquipmentsAccounting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:EquipmentsAccounting.windowsEquipmentsAccounting.windows  5   	   :    4EquipmentsAccounting.windows.WriteOfEquipmentsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:EquipmentsAccounting.windows     5   	     � Title$  WriteOfEquipmentsWindow��5   	   $��700                  ��6)   $	��720��5
      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $	��200 �6   .+�5   
   �� $5��/Оборудование (сотрудника)��6   $��0��5      $��	6,6,-6,35q�5      $��2��5      $��20(�5      	� 5   
   -   5         	�Name$ TypeRadioButton��5       d� 	IsChecked$
 TrueY�5      $��По типу��5      $
��Left=�5      $��
224,42,0,0q�5      $��0��5      $	��Top=�5       J� RenderTransformOrigin$ 0.236,0.735&�5       	� 5"   
   -   5        $ CharsRadioButton��5"      $)��#По характеристикам��5#      $
��Left=�5$      $��0��65   $	��Top=�5%      $��
308,42,0,0q�62   	� 5'   
   -   5        $ SerialNumRadioButton��5'      $*��$По серийному номеру��5(      $
��Left=�5)      $��
461,42,0,0q�5*      $	��Top=�5+      �� 5-   
   -   5         ��Name$ FilterTextBox��5-      $
��Left=�5.      $��	22,41,0,0q�5/       �� TextWrapping$
 Wrap=�50      $	��Top=�51      $	��187��52      $��12(�53      54      &   System.Windows.Controls.DataGrid  56   
   -   5          Name$* $EquipmentsOfSelectedEmployeeDataGrid��56      $��0��58         
IsReadOnly . 	   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$	 * 5:      $��0,72,0,0q�6'   ��57      �� 
   	CellStyle
 5;      �� 5<       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   .�5=      -   5        5>      5@        5B   
   -   5        $  SelectedEquipmentsDataGrid��5B       . $	 * 5E      $��
0,454,0,43q�6'   $��2��6;   ��5C      �� 
 5F      �� 5G      "  6   .�5H      -   5        5I      5K      �� 5L   
   -	   5         ��Name$ WriteOfButton��5L      $��Списать��5M      $��Center=�5N      $��	0,168,0,0q�5O      $��1��5P      $	��Top=�5Q      $��81��5R      $��14(�5S      5T      5U      5V      