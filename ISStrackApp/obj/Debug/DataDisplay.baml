   M S B A M L   `   `   `  ����F  BISStrackApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ISStrackAppISStrackApp  5          ISStrackApp.DataDisplay   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:ISStrackApp     5      ���	#FFF7F7F7.��5      �� $��2q�6   
����  �._�5   
   �   � ColumnDefinitions  5      �� 5      $��* �6#   6/   �� 5      $��* �6#   6/    � RowDefinitions 5      �� 5      $	��30* �6    6/   �� 5      $	��26* �6    �� 5      $	��35* �6    6/   �� 5      $	��26* �6    �� 5      $	��35* �6    6/   �� 5      $	��26* �6    �� 5      $	��35* �6    6/   �� 5      $	��26* �6    �� 5      $	��35* �6    6/   �� 5      $	��26* �6    6/   �� 5      $	��35* �6    6/   �� 5      $	��57* �6    6/   .+�5       �� 
�������
���   �$��0��64   $��12��6A   $��0��6S   $��2��6c   �� 5!      $��
>Longitude��6   $
��Left=�6*   $	��Top=�6E   
�������$��1��6p   $��2��6}   �� 5"      -   5         ��Name$ 	lon_label��5"      $��Right=�6*   $	��Top=�6F    �� 
IsReadOnly . $��2��6p   $��1��6}   �� 5$      $��	>Latitude��6   $
��Left=�6)   $	��Top=�6E   
�������$��3��6p   $��2��6}   �� 5%      -   5        $ 	lat_label��5%      $��Right=�6*   $	��Top=�6F    . $��4��6p   $��0��6}   $��2��6�   �� 5'      $��	>Altitude��6   $
��Left=�6)   $	��Top=�6D   
�������$��2��6p   $��5��6�   �� 5(      -   5        $ 	alt_label��5(      $��Right=�6*   $	��Top=�6F    . $��2��6q   $��6��6�   �� 5*      $��	>Velocity��6   $
��Left=�6)   $	��Top=�6D   
�������$��2��6o   $��7��6�   �� 5+      -   5        $ 	vel_label��5+      $��Right=�6*   $	��Top=�6F    . $��2��6p   $��8��6�   �� 5-      $��>Date & Time��6   $
��Left=�60   $	��Top=�6K   
�������$��2��6w   $��9��6�   �� 5.      -   5        $ 
time_label��5.      $��Right=�6+   $	��Top=�6H    . $��2��6r   $��10��6�   	� 50      -   5         	�Name$ km_radio��50      $��Km��6,   $��Center=�69   $	��Top=�6V   
������� d� 	IsChecked$
 trueY�6�   $��11��6�   $��0��6�   	� 51      -   5        $ 	mil_radio��51      $��Miles��6-   $��Center=�6=   $	��Top=�6Z   
�������$��11��6�   $��1��6�   53      54      55      