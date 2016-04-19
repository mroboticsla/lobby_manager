SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_alerts](
	[alert_id] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[alert_name] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[alert_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_com_comments](
	[com_id] [int] NOT NULL,
	[com_date] [datetime2](7) NOT NULL,
	[com_user] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[com_description] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[com_hidden] [int] NOT NULL,
	[com_type] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[com_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_dep_departments](
	[dep_id] [bigint] NOT NULL,
	[dep_name] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[dep_contact] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[dep_phone] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[dep_status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[dep_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_dev_clients](
	[dev_id] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[dev_name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[dev_station] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[dev_status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[dev_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_dev_stations](
	[dev_id] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[dev_name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[dev_status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[dev_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_doc_documents](
	[doc_id] [bigint] NOT NULL,
	[doc_name] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[doc_abreviature] [nchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[doc_is_national_idcard] [int] NOT NULL,
	[doc_status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[doc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_emp_employees](
	[emp_id] [int] NOT NULL,
	[emp_dep] [int] NULL,
	[emp_name] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[emp_lastname] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[emp_email] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[emp_phone] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[emp_notify] [int] NULL,
	[emp_notify_by_phone] [int] NULL,
	[emp_status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[emp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_img_images](
	[img_id] [int] NOT NULL,
	[img_visitor] [int] NOT NULL,
	[img_front] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[img_back] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[img_profile] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[img_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_lbl_labels](
	[lbl_desk] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[lbl_name] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lbl_serial] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lbl_equipment] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lbl_desc] [nchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lbl_owner] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_log_events](
	[log_id] [int] NOT NULL,
	[log_notification] [int] NOT NULL,
	[log_user] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[log_date] [datetime] NOT NULL,
	[log_visitor_record] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_menu](
	[menu_id] [int] NOT NULL,
	[menu_root] [int] NULL,
	[menu_label] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[menu_file] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[menu_status] [int] NULL,
	[menu_root_level] [int] NULL,
	[menu_icon] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_reg_equipment](
	[reg_id] [int] NOT NULL,
	[reg_type] [int] NOT NULL,
	[reg_quantity] [int] NOT NULL DEFAULT ((1)),
	[reg_serial] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[reg_desc] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[reg_visitor] [int] NOT NULL,
	[reg_status] [int] NOT NULL,
	[reg_last_update] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[reg_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_role_menu](
	[role_id] [int] NOT NULL,
	[role_menu] [int] NOT NULL,
	[role_access] [int] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_roles](
	[role_id] [int] NOT NULL,
	[role_name] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[role_level] [int] NULL,
	[role_status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_temp_images](
	[temp_desk] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[temp_front] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[temp_back] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[temp_profile] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[temp_ocr] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[temp_desk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_type_equipment](
	[type_id] [int] NOT NULL,
	[type_name] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[type_status] [int] NOT NULL,
	[type_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_type_notifications](
	[type_id] [int] NOT NULL,
	[type_label] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[type_class] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[type_access] [int] NOT NULL,
	[type_enabled] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_usr_users](
	[usr_id] [int] NOT NULL,
	[usr_role] [int] NULL,
	[usr_username] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[usr_password] [binary](20) NULL,
	[usr_status] [nchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[usr_name] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[usr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_vis_blacklist](
	[vis_id] [int] NOT NULL,
	[vis_document] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_lastname] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_alert_level] [nchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[vis_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_vis_visitors](
	[vis_id] [bigint] NOT NULL,
	[vis_name] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[vis_lastname] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[vis_doctype] [int] NULL,
	[vis_docnumber] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_company] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_phone] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_reason] [int] NOT NULL,
	[vis_description] [nchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_department] [int] NULL,
	[vis_internal_contact] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_date] [datetime] NOT NULL,
	[vis_status] [int] NOT NULL,
	[vis_image_record] [int] NULL,
	[vis_with_equipment] [int] NULL,
	[vis_checkout] [datetime] NULL,
	[vis_visitor_card] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[vis_in_charge] [nchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[vis_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_vrs_visit_reasons](
	[vrs_id] [bigint] NOT NULL,
	[vrs_name] [nchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[vrs_notification_type] [int] NOT NULL,
	[vrs_status] [int] NOT NULL,
	[vrs_date] [datetime] NOT NULL,
	[vrs_level] [int] NOT NULL,
	[vrs_require_document] [int] NOT NULL,
	[vrs_require_access] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[vrs_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
