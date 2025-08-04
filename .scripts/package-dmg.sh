#!/usr/bin/env bash
set -e

echo "Packaging as .dmg..."
create-dmg --volname Loopback \
	--volicon "build/Loopback.app/Contents/Resources/icon.icns" \
	--window-pos 200 120 \
	--window-size 800 400 \
	--icon-size 100 \
	--icon "Loopback.app" 200 190 \
	--hide-extension "Loopback.app" \
	--app-drop-link 600 185 \
	--codesign "$MACOS_CERTIFICATE_NAME" \
	Loopback.dmg \
	build
