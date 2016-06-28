#!/bin/sh

export PATH="/Library/Frameworks/Mono.framework/Commands/:/usr/local/bin/:$PATH"

exec "`dirname \"$0\"`/iFolder 3" $@ 
