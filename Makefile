test:
	echo "Error: no tests specified"

clean:
	git clean -xdf

deploy:
	git subtree push --prefix Assets/Plugins/Common-Components origin upm

deploy-force:
	git push origin `git subtree split --prefix Assets/Plugins/Common-Components master`:upm --force
