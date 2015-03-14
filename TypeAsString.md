# Introduction #

Defines (recursively) function `TypeAsString` that converts a meta-type (see [Meta](Meta.md)) to a character string. For instance, for meta-type `Function([ param: [ single: Array(Integer); ]; result: Void; ])` the result will be `"{ [integer] }"`.

# Code #
Notice how easy to follow the code is, mostly due to using the conditional expressions (**where**/**when**), as well as the recursion.
```

TypeAsString =
    { dataType: Meta::DataType ->
        typeInfoAsString(@Meta.TypeInfo(dataType)) where `use auxiliary function`
            letrec typeInfoAsString: [character] = `define typeInfoAsString recursively` 
                { typeInfo: Meta::TypeInfo ->
                    result where
                        let
                            any `only one branch will succeed`
                                let result = "void"! when typeInfo.compositeType# = 0?
                                let result = "integer"! when typeInfo.integerType?
                                let result = "character"! when typeInfo.characterType?
                                let result = join "[", baseType, "]" end! where baseType = string(typeInfo.arrayType)! `function string is defined below`
                                let result = `composite type`
                                    join "[ ", variants, " ]" end! 
                                where 
                                    let variants = `string consisting of all variants separated with |`
                                        foldl vv, v in fieldLists into join vv, " | ", v end! 
                                    where
                                        let fieldLists = `array of field lists as strings`
                                            array fieldList(typeInfo.compositeType(i)) 
                                            by i 
                                            to typeInfo.compositeType# `number of variants`
                                            !
                                        where
                                            fieldList = `get field list for a variant`
                                                { fields: [[ name: [character] dataType: Meta::DataType ]] -> 
                                                    lst where
                                                        unless
                                                            lst = `omit void type if variant has one field`
                                                                join fields(0).name, ":" end! when
                                                                    all
                                                                        fields# = 1?
                                                                        @Meta.TypeInfo(fields(0).dataType).compositeType# = 0?
                                                                    end
                                                        then
                                                            lst =
                                                                foldl ff, f in desc into join ff, " ", f end! where
                                                                    desc = `array of field descriptors`
                                                                        array
                                                                            join 
                                                                                fields(i).name, ": ", string(fields(i).dataType) 
                                                                            end 
                                                                        by i 
                                                                        to fields# 
                                                                        ! `desc`
                                                }! `fieldList`
                                let result = `function type`
                                    join "{ ", paramType, resultType, " }" end! 
                                where
                                    all
                                        unless `define paramType`
                                            let paramType = ""! when @Meta.TypeInfo(typeInfo.functionType.param.first).compositeType# = 0? `omit void parameter`
                                        then
                                            unless
                                                paramType = 
                                                    join 
                                                        string(typeInfo.functionType.param.first),
                                                        ", ",
                                                        string(typeInfo.functionType.param.second)
                                                    end 
                                                    ! `paramType`
                                            then
                                                paramType = string(typeInfo.functionType.param.single)!
                                        unless `define resultType`
                                            let resultType = ""! when @Meta.TypeInfo(typeInfo.functionType.result).compositeType# = 0? `omit void result together with ->`
                                        then
                                            resultType = join " -> ", string(typeInfo.functionType.result) end!
                                    end
                                unless
                                    let result =  `sequence type is a special case of an object type`
                                        join baseType, "*" end! 
                                    where 
                                        let baseType = 
                                            string(typeInfo.objectType.state)! 
                                        when 
                                            all 
                                                typeInfo.objectType.actions# = 1? `only one action`
                                                typeInfo.objectType.actions(0).name = "skip"? `the only action is 'skip'`
                                                @Meta.TypeInfo(typeInfo.objectType.actions(0).param).compositeType# = 0? `void parameter`
                                            end
                                then
                                    let result = `object type`
                                        join "obj ", stateType, " ", actions, " end" end! 
                                    where
                                        all
                                            stateType = string(typeInfo.objectType.state)!
                                            let actions =
                                                foldl aa, a in lst into join aa, " ", a end! 
                                            where
                                                lst =
                                                    array
                                                        action where
                                                            unless
                                                                let action = `omit void parameter`
                                                                    typeInfo.objectType.actions(i).name! 
                                                                when 
                                                                    @Meta.TypeInfo(typeInfo.objectType.actions(i).param).compositeType# = 0?
                                                            then
                                                                action = 
                                                                    join 
                                                                        typeInfo.objectType.actions(i).name, 
                                                                        "(", 
                                                                        string(typeInfo.objectType.actions(i).param), 
                                                                        ")" 
                                                                    end!
                                                    by i
                                                    to typeInfo.objectType.actions#
                                                    ! `lst`
                                        end `all`
                                let result = "<opaque>"! when typeInfo.opaqueType?
                            end `any`
                        where
                            string = { dt: Meta::DataType -> typeInfoAsString(@Meta.TypeInfo(dt)) }! `call typeInfoAsString recursively`
                }
            to Nesting(dataType) + 1
    }!
```