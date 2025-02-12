/*

service.h

Service structures.

--------------------------------------------------------------------------------
gSOAP XML Web services tools
Copyright (C) 2001-2004, Robert van Engelen, Genivia, Inc. All Rights Reserved.
This software is released under one of the following two licenses:
GPL or Genivia's license for commercial use.
--------------------------------------------------------------------------------
GPL license.

This program is free software; you can redistribute it and/or modify it under
the terms of the GNU General Public License as published by the Free Software
Foundation; either version 2 of the License, or (at your option) any later
version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
this program; if not, write to the Free Software Foundation, Inc., 59 Temple
Place, Suite 330, Boston, MA 02111-1307 USA

Author contact information:
engelen@genivia.com / engelen@acm.org
--------------------------------------------------------------------------------
A commercial use license is available from Genivia, Inc., contact@genivia.com
--------------------------------------------------------------------------------

*/

#ifndef SERVICE_H
#define SERVICE_H

#include "includes.h"
#include "wsdlH.h"

class Message
{ public:
    const char *name;
    const char *URI;
    soap__useChoice use;
    const char *encodingStyle;
    wsdl__message *message;
    const char *body_parts;
    wsdl__part *part;
    vector<soap__header> header;
    mime__multipartRelated *multipartRelated;	// MIME
    const char *layout;				// DIME
    const char *documentation;
    const char *ext_documentation;
    void generate(Types&, const char *sep, bool anonymous, bool remark);
};

typedef map<const char*, Message*, ltstr> MapOfStringToMessage;

class Operation
{ public:
    const char *prefix;
    const char *URI;
    const char *name;
    soap__styleChoice style;
    const char *parameterOrder;
    const char *soapAction;
    const char *input_name;
    const char *output_name;
    Message *input; 			// name, use, and parts
    Message *output;			// name, use, and parts
    vector<Message*> fault;
    const char *documentation;
    const char *operation_documentation;
    void generate(Types&);
};

class Service
{ public:
    const char *prefix;			// a gSOAP service has a unique namespace
    const char *URI;
    const char *name;			// binding name
    const char *type;			// portType
    SetOfString location;		// WSDL may specify multiple locations via <Port> -> <Binding>
    vector<Operation*> operation;
    MapOfStringToMessage header;
    MapOfStringToMessage headerfault;
    MapOfStringToMessage fault;
    MapOfStringToString service_documentation;
    MapOfStringToString port_documentation;
    MapOfStringToString binding_documentation;
    Service();
    void generate(Types&);
};

typedef map<const char*, Service*, ltstr> MapOfStringToService;

class Definitions
{ public:
    Types types;				// to process schema type information
    MapOfStringToService services;		// service information gathered
    Definitions();
    void collect(const wsdl__definitions&);
    void compile(const wsdl__definitions&);
  private:
    void analyze(const wsdl__definitions&);
    void generate();
};

#endif
