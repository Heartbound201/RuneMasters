// Copyright Epic Games, Inc. All Rights Reserved.

#include "RuneMastersGameMode.h"
#include "RuneMastersPlayerController.h"
#include "RuneMastersCharacter.h"
#include "UObject/ConstructorHelpers.h"

ARuneMastersGameMode::ARuneMastersGameMode()
{
	// use our custom PlayerController class
	PlayerControllerClass = ARuneMastersPlayerController::StaticClass();

	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/TopDownCPP/Blueprints/TopDownCharacter"));
	if (PlayerPawnBPClass.Class != nullptr)
	{
		DefaultPawnClass = PlayerPawnBPClass.Class;
	}
}