   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem    Epi.Web.SurveyManager.Client1   +Epi.Web.SurveyManager.Client.ResponseClient   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5my&clr-namespace:System;assembly=mscorlib       N� Title$  ResponseClient�� � 	Resources    CanUserAddRows1  CanUserDeleteRows1��%0   &	        &	      ��True����True��.���� $
��Auto=�.��� $
��Left=�$	��Top=� � RowDefinitions �� .+��� -   $��label2��$
��Bold"�$��Respose Client��$��39��$
��Left=�$��0,48,0,0q�$	��Top=�$	��112���� -   $��button1��$��View Download Client��$��30��$
��Left=�$��	10,10,0,0q�$	��Top=�$	��134���� -   $��ManageSurveyButton��$��Manage Existing Surveys��$��30��$
��Left=�$��
163,10,0,0q�$	��Top=�$	��140���� -   $��AddOrganization��$��Add Organization��$��30��$
��Left=�$��
317,10,0,0q�$	��Top=�$	��162���� -   $��	groupBox2��$
��Left=�$	��Top=����#FFCC00$
��Bold"�$��Response Info��$��13,67,66,672q�$	��600��.��� $
��Left=�$	��Top=�$	��635��.+��� -   $��label3��$��
Survey ID:��$��28��$
��Left=�$��
7,12,0,594q�$��126.313���� -   $��SurveyId�� �� AutoWordSelection . 
�������$��23��$
��Left=�$��
154,12,0,0q�$	��Top=�$��307.687���� -   $��label4��$��User Publish Key:��$��28��$
��Left=�$��
7,44,0,562q�$	��138���� -	   $��UserPublishKey�� . 
�������$��23��$��Right=�$��	0,49,84,0q�$	��Top=�$	��307��� $	��386��$��Stretch=�$��7,102,1,147q�$��Stretch=�.+��� -
   $��textBox1��$��85,118,196,248q� � DataContext ��  �� ElementName$ 	listView1�� �� Path$ SelectedItem����� $ Username��� -   $��textBox2��$��23��$��85,0,196,217q�$��Bottom=� �� $ 	listView1��$ SelectedItem����� $ Password��� -   $��label5��$��Key :��$��9,118,0,244q�$
��Left=�$��67���� -   $��label6��$��Value :��$��30��$��9,0,460,208q�$��Bottom=��� -   $��btnAdd��$	��Add��$��26��$��9,0,458,176q�$��Bottom=��� -   $��	btnUpdate��$��Update��$��26��$
��Left=�$��
95,0,0,176q�$��Bottom=�$��75���� -   $��	btnDelete��$��Delete��$��26��$
��Left=�$��185,0,0,176q�$��Bottom=�$��75���� -   $��btnClear��$��Clear��$
��27.5��$��270,0,196,175q�$��Bottom=��� -   $��button3��$��Insert Response��$��28��$
��Left=�$��412,225,0,0q�$	��Top=�$	��121��G� -   $��passwordBox1��$��137,225,139,133q� G� 	MaxLength$ 36���� -   $��label9��$��Organization Key:��$��28��$��14,226,411,132q��� -   $��Result��$	��120��$
��Left=�$��
17,260,0,0q�$	��Top=�$	��513�� �� �� -   $��	listView1��$��
6,6,13,274q�$	��100��$	��250������  �� View �� .P��� $	��Key��	 �� DisplayMemberBinding	 �� $	 Key��� $��Value��	 �� $ Value��� -   $��label1��$��Key/Values:��$��28��$
��Left=�$��7,74,0,0q�$	��Top=��� -   $��button2��$��Clear Response��$��23��$
��Left=�$��401,502,0,0q�$	��Top=�$	��136���� -   $��OpenResponse��$��Open Response��$
��Left=�$��
26,502,0,0q�$	��Top=�$	��119��