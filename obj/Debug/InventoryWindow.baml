   M S B A M L   `   `   `  ����  InventaireCannabis(    "InventaireCannabis.InventoryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  	Inventory��5   	   $	��500��6   $	��850��6(   .��5      � .+�5   
   ��  �� Orientation$ Vertical=�6   $��Right=�6,   $	��Top=�6H   $��10q�6`   .+�5      �� -   5         ��Name$ txtCount��5      $��	xx / 2000��6*   $��16(�6;   $
��Bold"�6I   $��0,0,0,10q�6[   �� 5      -   5        $ txtRemainingCapacity��5      $��Capacité restante : yy��66   $��16(�6T   $��0,0,0,10q�6b   �� 5	      -   5         ��Name$ btnModifyCapacity��5	      $��Modifier Capacité Max��60   $	��200��6p   5
      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgInventory��5         AutoGenerateColumns .     CanUserAddRows .  $��10,169,10,10q�6[      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 6P   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 61   �� *
PlantuleId+6P     5      $ Date Arrivée��6%   $	 * 6[   
 6;   �� *DateArrivee+6[     5      $ État Santé��6%   $	 * 6W   
 69   �� *	EtatSante+6W     5      $ 
Provenance��6%   $	 * 6X   
 69   �� *
Provenance+6X     5      $ Description��6%   $	 * 6Z   
 6:   �� *Description+6Z     5      $ Stade��6%   $	 * 6N   
 64   �� *Stade+6N     5      $ Entreposage��6%   $	 * 6Z   
 6:   �� *Entreposage+6Z     5      $
 Note��6%   $	 * 6L   
 63   �� *Note+6L   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ QR Code��6)   $
	 Auto 6:      CellTemplate 5      �� 5      .R�5      �� $��50��6C   $��50��6N   ��6$   �� *QrCodeImage+6N   5      5      5      5      5      