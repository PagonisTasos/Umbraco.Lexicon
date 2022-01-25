function lexiconBuilderController(lexiconBuilderResource, notificationsService) {

    var vm = this;

    vm.generate = generate;

    function generate() {
        lexiconBuilderResource.buildLexicon().then(
            (result) => notificationsService.success("Lexicon generated", "Lexicon.generated.cs was created successfully."),
            () => notificationsService.error("Generation Failed", "Generation is only available in development environment.")
        );
    }
}

angular.module("umbraco").controller("LexiconBuilderController", lexiconBuilderController);