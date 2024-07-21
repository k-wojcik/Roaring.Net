#!/bin/bash

vcpkg install --triplet=x64-linux-dynamic
mkdir -p libs/x64-linux/
cp ./vcpkg_installed/x64-linux-dynamic/lib/*.so ./libs/x64-linux/