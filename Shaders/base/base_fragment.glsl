    #version 460 core

    //based on vertex
    in vec3 Position;
    in vec3 Normal;
    in vec2 TexCoord;
    in vec3 Color;
    in vec3 Tangent;

    //cameara pos
    uniform vec3 viewPos;

    //Light
    struct Light {
        vec3 position;
        vec3 color;
        float intensity;
    };
    uniform int lightCount; // problems if >8
    uniform Light lights[8]; // Max 8
    uniform float ambientStrength = 0.1;
    uniform float specularStrength = 0.5;
    uniform float LinearFade = 0.09; //Attenuation
    uniform float QuadraticFade = 0.032; //Attenuation

    //material
    uniform sampler2D texture0; //albedo
    uniform sampler2D normalMap;
    uniform float shininess = 32.0;

    //out
    out vec4 FinalColor;

    vec3 CalculateLight(Light light, vec3 normal, vec3 fragPos, vec3 viewDir)
    {
        // Ambient
        vec3 ambient = ambientStrength * light.color * light.intensity;
        
        // Diffuse
        vec3 lightDir = normalize(light.position - fragPos);
        float diff = max(dot(normal, lightDir), 0.0);
        vec3 diffuse = diff * light.color * light.intensity;
        
        // Specular
        vec3 reflectDir = reflect(-lightDir, normal);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
        vec3 specular = specularStrength * spec * light.color * light.intensity;
        
        // Attenuation
        float distance = length(light.position - fragPos);
        float attenuation = 1.0 / (1.0 + LinearFade * distance + QuadraticFade * distance * distance);
        
        return ambient + (diffuse + specular) * attenuation;
    }

    // For normal mapping
    vec3 GetNormalFromMap(vec2 texCoord, vec3 normal, vec3 tangent)
    {
        vec3 normalMapValue = texture(normalMap, texCoord).rgb;
        normalMapValue = normalize(normalMapValue * 2.0 - 1.0);
        
        vec3 bitangent = normalize(cross(normal, tangent)); // third axis 
        mat3 TBN = mat3(tangent, bitangent, normal); //Tangent, Bitangent, Normal. This converts normals into world space
        
        return normalize(TBN * normalMapValue);
    }


    // uniform float edgeThreshold = 1.0; 
    // uniform vec3 edgeColor = vec3(1.0, 0.0, 0.0);     

    // Edge detection using world position differences
    // float DetectEdge(vec3 fragPos)
    // {
    //     // Calculate screen-space derivatives of the position
    //     vec3 ddx = dFdx(fragPos);
    //     vec3 ddy = dFdy(fragPos);
        
    //     // Calculate how much the position changes
    //     float diff = length(ddx) + length(ddy);
        
    //     // Normalize by distance from camera for consistent results
    //     float dist = length(fragPos - viewPos);
    //     diff = diff / (dist * 0.01); // Adjust multiplier for sensitivity
        
    //     // Return 1.0 if edge detected, 0.0 otherwise
    //     return step(edgeThreshold, diff);
    // }


    void main()
    {
        vec3 viewDir = normalize(viewPos - Position);
        
        //normal
        vec3 localNormal = Normal;
        if (textureSize(normalMap, 0).x > 0) 
        {
            localNormal = GetNormalFromMap(TexCoord, Normal, Tangent); 
        }

        //light
        vec3 light = vec3(0.0);
        for (int i = 0; i < lightCount; i++)
        {
            light += CalculateLight(lights[i], localNormal, Position, viewDir);
        }
        
        //texture
        vec4 textureColor = texture(texture0, TexCoord);
        
        vec3 finalColor = vec3(light * textureColor.rgb * Color);

        //draw
        // float edge = DetectEdge(Position);
        // finalColor = mix(finalColor, edgeColor, edge);
        
        FinalColor = vec4(finalColor, textureColor.a);
    }