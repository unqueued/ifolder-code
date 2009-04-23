#
# spec file for package ifolder3 (Version 3.7.2.@@BUILDNUM@@.1)
#
# Copyright (c) 2009 SUSE LINUX Products GmbH, Nuernberg, Germany.
#
# All modifications and additions to the file contributed by third parties
# remain the property of their copyright owners, unless otherwise agreed
# upon. The license for this file, and modifications and additions to the
# file, is the same license as for the pristine package itself (unless the
# license for the pristine package is not an Open Source License, in which
# case the license is the MIT License). An "Open Source License" is a
# license that conforms to the Open Source Definition (Version 1.9)
# published by the Open Source Initiative.
#
# Please submit bugfixes or comments via http://support.novell.com
#
# norootforbuild

Name:           ifolder-enterprise-plugins
BuildRequires:  compat-libstdc++ e2fsprogs e2fsprogs-devel gcc-c++ glib2 glib2-devel libflaim libflaim-devel libstdc++ libstdc++-devel libxml2 libxml2-devel mod_mono mono-core mono-data mono-devel mono-web pkgconfig xsp gtk-sharp2 glib-sharp2 ifolder3-enterprise
%define buildnum @@BUILDNUM@@

Url:            http://www.ifolder.com
%define prefix /usr
%define novell_lib /opt/novell/%_lib
%define sysconfdir /etc
License:        GNU General Public License (GPL) v2
Group:          Productivity/Networking/Novell
Autoreqprov:    on
Requires:       mono-core >= 1.1.8
Requires:       mono-data >= 1.1.8
Requires:       mono-web >= 1.1.8
Requires:       mod_mono
Requires:       ifolder3-enterprise > 3.7.0
#Obsoletes:      %{name} < %{version}
Version:        3.7.2.@@BUILDNUM@@.1
Release:        2
Summary:        Plugins adding additional capability to iFolder 3 enterprise server
Source:         ifolder-enterprise-plugins.tar.gz
BuildRoot:      %{_tmppath}/%{name}-%{version}-build
#=============================================================================
%description
Plugins for iFolder 3 enterprise server

%prep
export BUILDNUM=%{buildnum}
%setup -n %{name}
#=============================================================================

%build
export BUILDNUM=%{buildnum}
export EDIR_INCLUDE=/opt/novell/eDirectory/include
export EDIR_LIBDIR=/opt/novell/eDirectory/%_lib
export LIBDIR=%_lib
./autogen.sh --prefix=%{prefix} #--with-runasclient
make
#make dist
#=============================================================================

%install
export BUILDNUM=%{buildnum}
%{__rm} -rf $RPM_BUILD_ROOT
make DESTDIR=$RPM_BUILD_ROOT install
#============================================================================

%clean
%{__rm} -rf $RPM_BUILD_ROOT
#=============================================================================

%post
#=============================================================================
%files
%defattr(755,root,root)
%dir %{prefix}/share/plugins
%{prefix}/bin/iFolderLdapUserUpdate.sh
%{prefix}/bin/UserAdd.exe
/etc/iFolderLdapPlugin.ldif
/etc/iFolderADLdapPlugin.ldif
/etc/iFolderLdapPlugin.schema
/etc/simias/bill/modules/IdentityManagement.conf
/etc/simias/bill/modules/UserMovement.conf
%{prefix}/%_lib/simias/web/bin/UserMovement.dll
%{prefix}/%_lib/simias/web/bin/Simias.Identity.ADLdapProvider.dll
%{prefix}/%_lib/simias/web/bin/IdentityManagement.dll
%{prefix}/%_lib/simias/web/bin/Simias.Identity.LdapProvider.dll
%{prefix}/%_lib/simias/web/bin/Simias.Identity.OpenLdapProvider.dll
%{prefix}/share/plugins/COPYING

%changelog

