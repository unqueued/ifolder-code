/***********************************************************************
 *  $RCSfile$
 *
 *  Copyright (C) 2006 Novell, Inc.
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU General Public
 *  License as published by the Free Software Foundation; either
 *  version 2 of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public
 *  License along with this program; if not, write to the Free
 *  Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 *
 *  Author: Russ Young
 *
 ***********************************************************************/
#ifndef _IFDOMAIN_H_
#define _IFDOMAIN_H_

#include <vector>
#include "glibclient.h"
#include "simiasDomain_USCOREx0020_USCOREServiceSoapProxy.h"

// forward declarations
class IFDomain;
class IFDomainIterator;
class IFDomainList;
class XmlNode;
class ParseTree;
class GLIBCLIENT_API Domain;
class GLIBCLIENT_API std::vector<IFDomain*>;

class GLIBCLIENT_API IFDomain
{
	friend class IFDomainList;
private:
	// XML elements
	static gchar *EDomain;
	static gchar *EName;
	static gchar *EDescription;
	static gchar *EUser;
	static gchar *EHost;
	static gchar *EUrl;
	static gchar *EID;
	static gchar *EPW;
	static gchar *EPOB;

	// Members.
	Domain		m_DomainService;

public:
	// Properties
	gchar*		m_Name;
	gchar*		m_ID;
	gchar*		m_Description;
	gchar*		m_HomeUrl;
	gchar*		m_MasterUrl;
	gchar*		m_POBoxID;
	gchar*		m_UserName;
	gchar*		m_UserPassword;
	gchar*		m_UserID;
	
private:
	IFDomain();
	IFDomain(const gchar* userName, const gchar* password, const gchar* host);
	gboolean Serialize(FILE *pStream);
	static IFDomain* DeSerialize(ParseTree *tree, GNode *pDNode);
	
public:
	virtual ~IFDomain(void);
	static IFDomain& Add(const gchar* userName, const gchar* password, const gchar* host);
	int Remove();
	int Login();
	int Logout();
	static IFDomainIterator GetDomains();
	static IFDomain& GetDomainByID(const gchar *pID);
	static IFDomain& GetDomainByName(const gchar *pName);
};

class GLIBCLIENT_API IFDomainIterator
{
private:
	GArray				*m_List;
	guint				m_Index;
	
public:
	IFDomainIterator(GArray* list) {m_List = list; m_Index = 0; }
	virtual ~IFDomainIterator() {};
	void Reset() {m_Index = 0; }
	IFDomain* Next()
	{
		if (m_Index >= m_List->len)
			return NULL;
		IFDomain *pDomain = g_array_index(m_List, IFDomain*, m_Index);
		m_Index++;
		return pDomain;
	};
};

class IFDomainList
{
	friend class IFDomain;
private:
	// The list of domains should be small so I will
	// use an array to store the list.
	static			IFDomainList* m_Instance;
	gchar			*m_pFileName;
	GArray			*m_List;
	ParseTree		*m_ParseTree;
	static gchar	*EDomains;
	static gfloat	m_Version;
	
	IFDomainList(void);
	virtual ~IFDomainList(void);
	static IFDomainList* Instance();
	static void XmlStart(GMarkupParseContext *pContext, const gchar *pName, const gchar **pANames, const gchar **pAValues, gpointer userData, GError **ppError);
	static void XmlEnd(GMarkupParseContext *pContext, const gchar *pName, gpointer userData, GError **ppError);
	static void XmlText(GMarkupParseContext *pContext, const gchar *text, gsize textLen, gpointer userData, GError **ppError);
	static void XmlError(GMarkupParseContext *pContext, GError *pError, gpointer userData);
	static void Destroy(gpointer data);
	static void Insert(IFDomain *pDomain);
	static gboolean Remove(const gchar *id);
	static IFDomainIterator IFDomainList::GetIterator();
	static IFDomain& GetDomainByID(const gchar *pID);
	static IFDomain& GetDomainByName(const gchar *pName);
	void Save();
	void Restore();

public:
	static int Initialize();
};

class XmlNode
{
public:
	enum NodeType
	{
		Element,
		Attribute,
	};
	gchar	*m_Name;
	gchar	*m_Value;

	NodeType m_Type;
	XmlNode(gchar *name, NodeType type) { m_Name = name; m_Value = NULL; m_Type = type; }
	XmlNode(gchar *name, gchar *value, NodeType type) { m_Name = name; m_Value = value; m_Type = type; }
	virtual ~XmlNode() { g_free(m_Name); g_free(m_Value); }
};

class ParseTree
{
public:
	GNode		*m_CurrentNode;
	GNode		*m_RootNode;

	ParseTree();
	virtual ~ParseTree();
	void StartNode(const gchar *name);
	void EndNode();
	void AddText(const gchar *text, gsize len);
	void AddAttribute(const gchar *name, const gchar *value);
	GNode* FindChild(GNode *parent, gchar* name, XmlNode::NodeType type);
	GNode* FindSibling(GNode *sibling, gchar* name, XmlNode::NodeType type);

};

#endif //_IFDOMAIN_H_
