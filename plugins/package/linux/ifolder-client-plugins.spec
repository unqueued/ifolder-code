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

Name:           ifolder-client-plugins
BuildRequires:  aspell-en cairo cairo-devel compat-libstdc++ gdk-pixbuf glitz-devel libflaim libflaim-devel gnutls-devel gtk-sharp2 gtk-sharp2-complete gtk2 gtk2-devel libglade2-devel libgnomeprintui-devel libgnomeui-devel libpng-devel libwnck-devel log4net mDNSResponder mono-core mono-data mono-devel mono-web pango pango-devel simias xsp gcc-c++ simias ifolder3 e2fsprogs-devel
%define buildnum @@BUILDNUM@@

URL:            http://www.ifolder.com
%define prefix /usr
%define sysconfdir /etc
License:        GNU General Public License (GPL) v2 
Group:          System/GUI/Other
Autoreqprov:    on
Requires:       simias > 1.7.0
Requires:       ifolder3 > 3.7.0
Requires:       gconf-sharp2
Requires:       gnome-sharp2
Requires:       gtk-sharp2
Requires:       xsp
#Obsoletes:      %{name} < %{version}
Version:        3.7.2.@@BUILDNUM@@.1
Release:        1
Summary:        Plugins adding additional capability to iFolder 3 client
Source:         %{name}.tar.gz
BuildRoot:      %{_tmppath}/%{name}-%{version}-build
#=============================================================================

%description
Plugins adding additional capability to iFolder 3 client

%prep
export BUILDNUM=%{buildnum}
%setup -n %{name}
#=============================================================================

%build
export BUILDNUM=%{buildnum}
export CFLAGS="$RPM_OPT_FLAGS -fno-strict-aliasing"
export CXXFLAGS="$RPM_OPT_FLAGS -fno-strict-aliasing"
./autogen.sh --prefix=%{prefix} --with-clientplugins
make
#make dist
#=============================================================================

%install
make DESTDIR=$RPM_BUILD_ROOT install
#=============================================================================

%post

%preun
#=============================================================================

%files
%defattr(755,root,root)
%dir %{prefix}/%_lib/plugins/
%dir %{prefix}/share/plugins/
%{prefix}/%_lib/plugins/*
%{prefix}/bin/AutoAccount.xsd
%{prefix}/bin/AutoAccount.xml
%{prefix}/share/plugins/COPYING

%changelog -n ifolder3
