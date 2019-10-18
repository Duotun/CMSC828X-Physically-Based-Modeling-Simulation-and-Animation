#include<iostream>
#include "Header.h"

extern "C" {
	float FooPluginFunction() {
		return 5.0f;
	}
}